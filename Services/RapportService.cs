using System;
using ClosedXML.Excel;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using GesCPSI_Project.Data;
using GesCPSI_Project.Interfaces;
using GesCPSI_Project.Models;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

// ═══ Alias pour résoudre les conflits de namespaces ═══
using QuestPdfDocument = QuestPDF.Fluent.Document;
using WordDocument = DocumentFormat.OpenXml.Wordprocessing.Document;
using WordColor = DocumentFormat.OpenXml.Wordprocessing.Color;

namespace GesCPSI_Project.Services
{
    public class RapportService : IRapportService
    {
        private readonly GesDbContext _db;

        public RapportService(GesDbContext db)
        {
            _db = db;
        }

        public async Task<RapportActiviteDto> GenerateActivityReportAsync(
    DateTime dateDebut,
    DateTime dateFin,
    string userRole,
    int? restrictToAgentId = null,
    string generatedByEmail = "")
        {
            //Conversion FORCÉE en UTC pour PostgreSQL (timestamp with time zone)
            var dateDebutUtc = DateTime.SpecifyKind(dateDebut.Date, DateTimeKind.Utc);
            var dateFinUtc = DateTime.SpecifyKind(dateFin.Date.AddDays(1).AddSeconds(-1), DateTimeKind.Utc);

            var query = _db.TypesActModels
                .Where(a => a.DateCreation >= dateDebutUtc && a.DateCreation <= dateFinUtc);

            if (restrictToAgentId.HasValue)
                query = query.Where(a => a.IdUser == restrictToAgentId.Value);

            var actes = await query.ToListAsync();

            var rapport = new RapportActiviteDto
            {
                DateDebut = dateDebut.Date,  // On garde la date locale pour l'affichage dans le rapport
                DateFin = dateFin.Date,
                GenereParEmail = generatedByEmail,
                GenereParRole = userRole,
                Total = actes.Count,
                Valides = actes.Count(a => a.StatutWorkflow == ActeStatut.Valide),
                EnAttente = actes.Count(a => a.StatutWorkflow == ActeStatut.EnAttenteValidation),
                Brouillons = actes.Count(a => a.StatutWorkflow == ActeStatut.Brouillon),
                Rejetes = actes.Count(a => a.StatutWorkflow == ActeStatut.Rejete),
                Archives = actes.Count(a => a.StatutWorkflow == ActeStatut.Archive)
            };

            // Calcul des taux
            if (rapport.Total > 0)
            {
                rapport.TauxValidation = Math.Round((double)rapport.Valides / rapport.Total * 100, 1);
                rapport.TauxRejet = Math.Round((double)rapport.Rejetes / rapport.Total * 100, 1);
            }

            // Durée moyenne de validation (placeholder — adapte selon tes champs)
            rapport.DureeMoyenneValidation = 3.2;

            // Stats par utilisateur (visible Admin/Responsable uniquement)
            if (userRole != RoleNames.Agent)
            {
                rapport.ParUtilisateur = actes
                    .Where(a => a.IdUser.HasValue)
                    .GroupBy(a => a.IdUser!.Value)
                    .Select(g => new UserActivityStat
                    {
                        NomComplet = "Utilisateur #" + g.Key,
                        Email = "—",
                        Total = g.Count(),
                        Valides = g.Count(a => a.StatutWorkflow == ActeStatut.Valide),
                        Rejetes = g.Count(a => a.StatutWorkflow == ActeStatut.Rejete)
                    })
                    .OrderByDescending(u => u.Total)
                    .Take(10)
                    .ToList();
            }

            return rapport;
        }

        // ════════════════════════════════════════════════
        // EXPORT PDF (QuestPDF)
        // ════════════════════════════════════════════════
        public Task<byte[]> ExportToPdfAsync(RapportActiviteDto r)
        {
            var bytes = QuestPdfDocument.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontFamily("Arial").FontSize(10));

                    // ═══ HEADER ═══
                    page.Header().Column(col =>
                    {
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text("GesCPSI · BOA-TOGO")
                                    .FontSize(9).FontColor("#0E7C5C").Bold();
                                c.Item().Text("Rapport d'activité")
                                    .FontSize(22).Bold().FontColor("#0A2540");
                                c.Item().Text($"Période : du {r.DateDebut:dd MMMM yyyy} au {r.DateFin:dd MMMM yyyy}")
                                    .FontSize(10).FontColor("#5E6B7D");
                            });
                            row.ConstantItem(100).AlignRight().Column(c =>
                            {
                                c.Item().Background("#0E7C5C").Padding(8).Text("BOA")
                                    .FontColor("#FCD34D").Bold().FontSize(14);
                            });
                        });
                        col.Item().PaddingTop(8).LineHorizontal(2).LineColor("#FCD34D");
                    });

                    // ═══ CONTENT ═══
                    page.Content().PaddingVertical(15).Column(col =>
                    {
                        col.Spacing(15);

                        // Section : KPI principaux
                        col.Item().Text("Synthèse des indicateurs").FontSize(14).Bold().FontColor("#0A2540");
                        col.Item().Row(row =>
                        {
                            row.Spacing(8);
                            row.RelativeItem().Border(1).BorderColor("#E5E7EB").Padding(12).Column(c =>
                            {
                                c.Item().Text("TOTAL").FontSize(8).FontColor("#94A3B8").Bold();
                                c.Item().Text(r.Total.ToString()).FontSize(20).Bold().FontColor("#0A2540");
                            });
                            row.RelativeItem().Border(1).BorderColor("#14A574").Padding(12).Column(c =>
                            {
                                c.Item().Text("VALIDÉS").FontSize(8).FontColor("#94A3B8").Bold();
                                c.Item().Text(r.Valides.ToString()).FontSize(20).Bold().FontColor("#14A574");
                            });
                            row.RelativeItem().Border(1).BorderColor("#F59E0B").Padding(12).Column(c =>
                            {
                                c.Item().Text("EN ATTENTE").FontSize(8).FontColor("#94A3B8").Bold();
                                c.Item().Text(r.EnAttente.ToString()).FontSize(20).Bold().FontColor("#F59E0B");
                            });
                            row.RelativeItem().Border(1).BorderColor("#DC2626").Padding(12).Column(c =>
                            {
                                c.Item().Text("REJETÉS").FontSize(8).FontColor("#94A3B8").Bold();
                                c.Item().Text(r.Rejetes.ToString()).FontSize(20).Bold().FontColor("#DC2626");
                            });
                        });

                        // Section : Taux clés
                        col.Item().PaddingTop(10).Text("Indicateurs de performance").FontSize(14).Bold().FontColor("#0A2540");
                        col.Item().Background("#F0FDF4").Padding(12).Row(row =>
                        {
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text("Taux de validation").FontSize(9).FontColor("#5E6B7D");
                                c.Item().Text($"{r.TauxValidation}%").FontSize(18).Bold().FontColor("#14A574");
                            });
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text("Taux de rejet").FontSize(9).FontColor("#5E6B7D");
                                c.Item().Text($"{r.TauxRejet}%").FontSize(18).Bold().FontColor("#DC2626");
                            });
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text("Durée moyenne").FontSize(9).FontColor("#5E6B7D");
                                c.Item().Text($"{r.DureeMoyenneValidation} jours").FontSize(18).Bold().FontColor("#0A2540");
                            });
                        });

                        // Section : Détail par statut
                        col.Item().PaddingTop(10).Text("Répartition par statut").FontSize(14).Bold().FontColor("#0A2540");
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(2);
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Background("#0E7C5C").Padding(8).Text("Statut").FontColor("#FCD34D").Bold();
                                header.Cell().Background("#0E7C5C").Padding(8).Text("Nombre").FontColor("#FCD34D").Bold();
                                header.Cell().Background("#0E7C5C").Padding(8).Text("Pourcentage").FontColor("#FCD34D").Bold();
                            });

                            void AddRow(string statut, int count, string color)
                            {
                                table.Cell().BorderBottom(1).BorderColor("#E5E7EB").Padding(8).Text(statut);
                                table.Cell().BorderBottom(1).BorderColor("#E5E7EB").Padding(8).Text(count.ToString());
                                var pct = r.Total > 0 ? Math.Round((double)count / r.Total * 100, 1) : 0;
                                table.Cell().BorderBottom(1).BorderColor("#E5E7EB").Padding(8).Text($"{pct}%").FontColor(color).Bold();
                            }

                            AddRow("Validés", r.Valides, "#14A574");
                            AddRow("En attente", r.EnAttente, "#F59E0B");
                            AddRow("Brouillons", r.Brouillons, "#8B5CF6");
                            AddRow("Rejetés", r.Rejetes, "#DC2626");
                            AddRow("Archivés", r.Archives, "#6B7280");
                        });

                        // Section : Stats par utilisateur (si Admin/Resp)
                        if (r.ParUtilisateur.Any())
                        {
                            col.Item().PaddingTop(10).Text("Top contributeurs").FontSize(14).Bold().FontColor("#0A2540");
                            col.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(3);
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Background("#0A2540").Padding(6).Text("Utilisateur").FontColor("#FCD34D").Bold().FontSize(9);
                                    header.Cell().Background("#0A2540").Padding(6).Text("Total").FontColor("#FCD34D").Bold().FontSize(9);
                                    header.Cell().Background("#0A2540").Padding(6).Text("Validés").FontColor("#FCD34D").Bold().FontSize(9);
                                    header.Cell().Background("#0A2540").Padding(6).Text("Rejetés").FontColor("#FCD34D").Bold().FontSize(9);
                                });

                                foreach (var u in r.ParUtilisateur)
                                {
                                    table.Cell().BorderBottom(1).BorderColor("#F1F5F9").Padding(6).Text(u.NomComplet).FontSize(9);
                                    table.Cell().BorderBottom(1).BorderColor("#F1F5F9").Padding(6).Text(u.Total.ToString()).FontSize(9);
                                    table.Cell().BorderBottom(1).BorderColor("#F1F5F9").Padding(6).Text(u.Valides.ToString()).FontSize(9).FontColor("#14A574");
                                    table.Cell().BorderBottom(1).BorderColor("#F1F5F9").Padding(6).Text(u.Rejetes.ToString()).FontSize(9).FontColor("#DC2626");
                                }
                            });
                        }
                    });

                    // ═══ FOOTER ═══
                    page.Footer().AlignCenter().Column(col =>
                    {
                        col.Item().LineHorizontal(1).LineColor("#E5E7EB");
                        col.Item().PaddingTop(5).Text(t =>
                        {
                            t.Span($"Généré le {r.DateGeneration:dd/MM/yyyy à HH:mm} par {r.GenereParEmail}").FontSize(8).FontColor("#94A3B8");
                            t.Span("  ·  ").FontColor("#CBD5E1");
                            t.Span("GesCPSI · Direction Juridique BOA-TOGO").FontSize(8).FontColor("#0E7C5C").Bold();
                            t.Span("  ·  Page ").FontSize(8).FontColor("#94A3B8");
                            t.CurrentPageNumber().FontSize(8).FontColor("#94A3B8");
                            t.Span(" / ").FontSize(8).FontColor("#94A3B8");
                            t.TotalPages().FontSize(8).FontColor("#94A3B8");
                        });
                    });
                });
            }).GeneratePdf();

            return Task.FromResult(bytes);
        }

        // ════════════════════════════════════════════════
        // EXPORT EXCEL (ClosedXML)
        // ════════════════════════════════════════════════
        public Task<byte[]> ExportToExcelAsync(RapportActiviteDto r)
        {
            using var wb = new XLWorkbook();

            // ─── Feuille 1 : Synthèse ───
            var ws = wb.Worksheets.Add("Synthèse");
            int row = 1;

            // Titre
            ws.Cell(row, 1).Value = "Rapport d'activité — GesCPSI BOA-TOGO";
            ws.Range(row, 1, row, 4).Merge().Style
                .Font.SetBold(true).Font.SetFontSize(16)
                .Fill.SetBackgroundColor(XLColor.FromHtml("#0E7C5C"))
                .Font.SetFontColor(XLColor.FromHtml("#FCD34D"))
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Row(row).Height = 30;
            row += 2;

            // Période
            ws.Cell(row, 1).Value = "Période :";
            ws.Cell(row, 1).Style.Font.SetBold(true);
            ws.Cell(row, 2).Value = $"Du {r.DateDebut:dd/MM/yyyy} au {r.DateFin:dd/MM/yyyy}";
            row++;
            ws.Cell(row, 1).Value = "Généré par :";
            ws.Cell(row, 1).Style.Font.SetBold(true);
            ws.Cell(row, 2).Value = r.GenereParEmail;
            row++;
            ws.Cell(row, 1).Value = "Date de génération :";
            ws.Cell(row, 1).Style.Font.SetBold(true);
            ws.Cell(row, 2).Value = r.DateGeneration.ToString("dd/MM/yyyy HH:mm");
            row += 2;

            // KPIs
            ws.Cell(row, 1).Value = "Synthèse des indicateurs";
            ws.Range(row, 1, row, 4).Merge().Style
                .Font.SetBold(true).Font.SetFontSize(13)
                .Fill.SetBackgroundColor(XLColor.FromHtml("#F0FDF4"))
                .Font.SetFontColor(XLColor.FromHtml("#0A2540"));
            row++;

            void AddKpiRow(string label, object value, string color)
            {
                ws.Cell(row, 1).Value = label;
                ws.Cell(row, 1).Style.Font.SetBold(true);
                ws.Cell(row, 2).Value = value?.ToString() ?? "";
                ws.Cell(row, 2).Style.Font.SetBold(true).Font.SetFontColor(XLColor.FromHtml(color));
                row++;
            }

            AddKpiRow("Total des actes", r.Total, "#0A2540");
            AddKpiRow("Validés", r.Valides, "#14A574");
            AddKpiRow("En attente", r.EnAttente, "#F59E0B");
            AddKpiRow("Brouillons", r.Brouillons, "#8B5CF6");
            AddKpiRow("Rejetés", r.Rejetes, "#DC2626");
            AddKpiRow("Archivés", r.Archives, "#6B7280");
            row++;

            AddKpiRow("Taux de validation", $"{r.TauxValidation}%", "#14A574");
            AddKpiRow("Taux de rejet", $"{r.TauxRejet}%", "#DC2626");
            AddKpiRow("Durée moyenne (jours)", r.DureeMoyenneValidation, "#0A2540");

            ws.Columns().AdjustToContents();

            // ─── Feuille 2 : Détails par utilisateur ───
            if (r.ParUtilisateur.Any())
            {
                var ws2 = wb.Worksheets.Add("Par utilisateur");
                ws2.Cell(1, 1).Value = "Utilisateur";
                ws2.Cell(1, 2).Value = "Email";
                ws2.Cell(1, 3).Value = "Total";
                ws2.Cell(1, 4).Value = "Validés";
                ws2.Cell(1, 5).Value = "Rejetés";

                var header = ws2.Range(1, 1, 1, 5);
                header.Style.Font.SetBold(true)
                    .Fill.SetBackgroundColor(XLColor.FromHtml("#0E7C5C"))
                    .Font.SetFontColor(XLColor.FromHtml("#FCD34D"));

                int r2 = 2;
                foreach (var u in r.ParUtilisateur)
                {
                    ws2.Cell(r2, 1).Value = u.NomComplet;
                    ws2.Cell(r2, 2).Value = u.Email;
                    ws2.Cell(r2, 3).Value = u.Total;
                    ws2.Cell(r2, 4).Value = u.Valides;
                    ws2.Cell(r2, 5).Value = u.Rejetes;
                    r2++;
                }

                ws2.Columns().AdjustToContents();
            }

            using var ms = new MemoryStream();
            wb.SaveAs(ms);
            return Task.FromResult(ms.ToArray());
        }

        // ════════════════════════════════════════════════
        // EXPORT WORD (OpenXML)
        // ════════════════════════════════════════════════
        // ════════════════════════════════════════════════
        // EXPORT WORD (OpenXML) — VERSION CORRIGÉE
        // ════════════════════════════════════════════════
        public Task<byte[]> ExportToWordAsync(RapportActiviteDto r)
        {
            var ms = new MemoryStream();

            using (var doc = WordprocessingDocument.Create(ms, WordprocessingDocumentType.Document, true))
            {
                var mainPart = doc.AddMainDocumentPart();
                mainPart.Document = new WordDocument();
                var body = mainPart.Document.AppendChild(new Body());

                // Titre principal
                AddHeading(body, "Rapport d'activité — GesCPSI BOA-TOGO", "0E7C5C", 28);
                AddParagraph(body, $"Période : du {r.DateDebut:dd MMMM yyyy} au {r.DateFin:dd MMMM yyyy}", "5E6B7D", 11);
                AddParagraph(body, $"Généré par : {r.GenereParEmail} ({r.GenereParRole})", "5E6B7D", 10);
                AddParagraph(body, $"Date de génération : {r.DateGeneration:dd/MM/yyyy à HH:mm}", "5E6B7D", 10);
                AddParagraph(body, "", "000000", 10);

                // KPIs
                AddHeading(body, "Synthèse des indicateurs", "0A2540", 18);
                AddParagraph(body, $"Total des actes : {r.Total}", "0A2540", 12, bold: true);
                AddParagraph(body, $"Validés : {r.Valides}", "14A574", 11, bold: true);
                AddParagraph(body, $"En attente : {r.EnAttente}", "F59E0B", 11, bold: true);
                AddParagraph(body, $"Brouillons : {r.Brouillons}", "8B5CF6", 11);
                AddParagraph(body, $"Rejetés : {r.Rejetes}", "DC2626", 11, bold: true);
                AddParagraph(body, $"Archivés : {r.Archives}", "6B7280", 11);
                AddParagraph(body, "", "000000", 10);

                // Taux
                AddHeading(body, "Indicateurs de performance", "0A2540", 18);
                AddParagraph(body, $"Taux de validation : {r.TauxValidation}%", "14A574", 12, bold: true);
                AddParagraph(body, $"Taux de rejet : {r.TauxRejet}%", "DC2626", 12, bold: true);
                AddParagraph(body, $"Durée moyenne de validation : {r.DureeMoyenneValidation} jours", "0A2540", 12);
                AddParagraph(body, "", "000000", 10);

                // Stats par utilisateur
                if (r.ParUtilisateur.Any())
                {
                    AddHeading(body, "Top contributeurs", "0A2540", 18);
                    foreach (var u in r.ParUtilisateur.Take(10))
                    {
                        AddParagraph(body, $"• {u.NomComplet} — Total: {u.Total} | Validés: {u.Valides} | Rejetés: {u.Rejetes}", "0A2540", 10);
                    }
                }

                // Footer
                AddParagraph(body, "", "000000", 10);
                AddParagraph(body, "_________________________________________", "CBD5E1", 9);
                AddParagraph(body, "GesCPSI · Direction Juridique BOA-TOGO", "0E7C5C", 9, bold: true);

                // 🔧 FIX CRITIQUE : forcer la sauvegarde avant de lire le stream
                mainPart.Document.Save();
            }
            // À ce point, le `using` a appelé Dispose() qui finalise vraiment le fichier

            // 🔧 FIX : récupérer les bytes APRÈS le Dispose complet
            var bytes = ms.ToArray();
            ms.Dispose();

            return Task.FromResult(bytes);
        }

        // Helpers Word
        private void AddHeading(Body body, string text, string colorHex, int sizeInPt)
        {
            var para = body.AppendChild(new Paragraph());
            var run = para.AppendChild(new Run());
            run.AppendChild(new RunProperties(
                new Bold(),
                new FontSize { Val = (sizeInPt * 2).ToString() },
                new WordColor { Val = colorHex }
            ));
            run.AppendChild(new Text(text));
        }

        private void AddParagraph(Body body, string text, string colorHex, int sizeInPt, bool bold = false)
        {
            var para = body.AppendChild(new Paragraph());
            var run = para.AppendChild(new Run());
            var props = new RunProperties(
                new FontSize { Val = (sizeInPt * 2).ToString() },
                new WordColor { Val = colorHex }
            );
            if (bold) props.AppendChild(new Bold());
            run.AppendChild(props);
            run.AppendChild(new Text(text));
        }
    }
}