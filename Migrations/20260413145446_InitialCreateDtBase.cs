using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GesCPSI_Project.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateDtBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AjoutActModels",
                columns: table => new
                {
                    IdAjout = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NomAct = table.Column<string>(type: "text", nullable: false),
                    LibelleAct = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CategorieClient = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AjoutActModels", x => x.IdAjout);
                });

            migrationBuilder.CreateTable(
                name: "BanqueModels",
                columns: table => new
                {
                    IdBnq = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NomRaisonSocialBnq = table.Column<string>(type: "text", nullable: false),
                    SigleBnq = table.Column<string>(type: "text", nullable: false),
                    FormeJuridique = table.Column<string>(type: "text", nullable: false),
                    CapitalSocial = table.Column<string>(type: "text", nullable: false),
                    SiegeSocial = table.Column<string>(type: "text", nullable: false),
                    RegistreCommerce = table.Column<string>(type: "text", nullable: false),
                    Swift = table.Column<string>(type: "text", nullable: false),
                    TelBnq = table.Column<string>(type: "text", nullable: false),
                    EmailBnq = table.Column<string>(type: "text", nullable: false),
                    RepresentantBnq = table.Column<string>(type: "text", nullable: false),
                    FonctionRepresentant = table.Column<string>(type: "text", nullable: false),
                    NomCollaborateur = table.Column<string>(type: "text", nullable: false),
                    VisaCollaborateur = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BanqueModels", x => x.IdBnq);
                });

            migrationBuilder.CreateTable(
                name: "ClientModels",
                columns: table => new
                {
                    IdClt = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TypeClient = table.Column<string>(type: "text", nullable: false),
                    NomRaisonsociale = table.Column<string>(type: "text", nullable: false),
                    NomRepresentant = table.Column<string>(type: "text", nullable: false),
                    Nom = table.Column<string>(type: "text", nullable: false),
                    Prenoms = table.Column<string>(type: "text", nullable: false),
                    Sexe = table.Column<string>(type: "text", nullable: false),
                    NumCompte = table.Column<string>(type: "text", nullable: false),
                    NumRCCM = table.Column<string>(type: "text", nullable: false),
                    DateNaiss = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LieuNaiss = table.Column<string>(type: "text", nullable: false),
                    PaysNaiss = table.Column<string>(type: "text", nullable: false),
                    Nationalite = table.Column<string>(type: "text", nullable: false),
                    Adressecomplet = table.Column<string>(type: "text", nullable: false),
                    VilleCommune = table.Column<string>(type: "text", nullable: false),
                    PaysResidn = table.Column<string>(type: "text", nullable: false),
                    BPClt = table.Column<string>(type: "text", nullable: false),
                    TelBur = table.Column<string>(type: "text", nullable: false),
                    TelCel = table.Column<string>(type: "text", nullable: false),
                    TelDom = table.Column<string>(type: "text", nullable: false),
                    ProfessionClt = table.Column<string>(type: "text", nullable: false),
                    SituationMatrim = table.Column<string>(type: "text", nullable: false),
                    NomConjoint = table.Column<string>(type: "text", nullable: false),
                    DateMariage = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LieuMariage = table.Column<string>(type: "text", nullable: false),
                    RegimeMatrim = table.Column<string>(type: "text", nullable: false),
                    TypePieceID = table.Column<string>(type: "text", nullable: false),
                    NumPieceID = table.Column<string>(type: "text", nullable: false),
                    DateDelivPiece = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LieuDelivPiece = table.Column<string>(type: "text", nullable: false),
                    PersonDelivPiece = table.Column<string>(type: "text", nullable: false),
                    CopiePieceJointe = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientModels", x => x.IdClt);
                });

            migrationBuilder.CreateTable(
                name: "RolesClientActModels",
                columns: table => new
                {
                    IdRole = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Libelle = table.Column<string>(type: "text", nullable: false),
                    MentionManuelle = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolesClientActModels", x => x.IdRole);
                });

            migrationBuilder.CreateTable(
                name: "UserModel",
                columns: table => new
                {
                    IdUser = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NameUser = table.Column<string>(type: "text", nullable: false),
                    SurnameUser = table.Column<string>(type: "text", nullable: false),
                    EmailUser = table.Column<string>(type: "text", nullable: false),
                    KeycloakId = table.Column<string>(type: "text", nullable: false),
                    Departement = table.Column<string>(type: "text", nullable: false),
                    Fonction = table.Column<string>(type: "text", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateModification = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserModel", x => x.IdUser);
                });

            migrationBuilder.CreateTable(
                name: "TypesActModels",
                columns: table => new
                {
                    IdActe = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NomTypesActe = table.Column<string>(type: "text", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Statut = table.Column<string>(type: "text", nullable: false),
                    DateMaj = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MotifRejet = table.Column<string>(type: "text", nullable: true),
                    DateValidation = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateRejet = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ValidateurId = table.Column<int>(type: "integer", nullable: true),
                    FichierValidationPath = table.Column<string>(type: "text", nullable: true),
                    PdfGenerePath = table.Column<string>(type: "text", nullable: true),
                    PdfSignePath = table.Column<string>(type: "text", nullable: true),
                    JsonSnapshotPath = table.Column<string>(type: "text", nullable: true),
                    DateGenerationPdf = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateUploadSignature = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IdAjout = table.Column<int>(type: "integer", nullable: false),
                    IdUser = table.Column<int>(type: "integer", nullable: false),
                    IdBnq = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypesActModels", x => x.IdActe);
                    table.ForeignKey(
                        name: "FK_TypesActModels_AjoutActModels_IdAjout",
                        column: x => x.IdAjout,
                        principalTable: "AjoutActModels",
                        principalColumn: "IdAjout",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TypesActModels_BanqueModels_IdBnq",
                        column: x => x.IdBnq,
                        principalTable: "BanqueModels",
                        principalColumn: "IdBnq",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TypesActModels_UserModel_IdUser",
                        column: x => x.IdUser,
                        principalTable: "UserModel",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AutorisationModels",
                columns: table => new
                {
                    IdAutorisation = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NumeroAutorisation = table.Column<string>(type: "text", nullable: false),
                    DateAutorisation = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LieuAutorisation = table.Column<string>(type: "text", nullable: false),
                    MontantMaxAutorise = table.Column<decimal>(type: "numeric", nullable: false),
                    ConditionsAutorisation = table.Column<string>(type: "text", nullable: false),
                    IdActe = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutorisationModels", x => x.IdAutorisation);
                    table.ForeignKey(
                        name: "FK_AutorisationModels_TypesActModels_IdActe",
                        column: x => x.IdActe,
                        principalTable: "TypesActModels",
                        principalColumn: "IdActe",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CautionnementModels",
                columns: table => new
                {
                    IdCautionnement = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NomTypesActe = table.Column<string>(type: "text", nullable: false),
                    NumeroCautionmt = table.Column<string>(type: "text", nullable: false),
                    MontantCautionne = table.Column<decimal>(type: "numeric", nullable: false),
                    MontantLettreCautionmt = table.Column<string>(type: "text", nullable: false),
                    DateSignatureCautionmt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateEffetCautionmt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateExpirationCautionmt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LieuSignatureCautionmt = table.Column<string>(type: "text", nullable: false),
                    EtatCautionmt = table.Column<string>(type: "text", nullable: false),
                    ConditionsRevocationCautionmt = table.Column<string>(type: "text", nullable: false),
                    ObligationsBanque = table.Column<string>(type: "text", nullable: false),
                    ObligationsCaution = table.Column<string>(type: "text", nullable: false),
                    ClauseSubrogation = table.Column<string>(type: "text", nullable: false),
                    ClauseNonNovation = table.Column<string>(type: "text", nullable: false),
                    ElectionDomicile = table.Column<string>(type: "text", nullable: false),
                    ReglementDifferend = table.Column<string>(type: "text", nullable: false),
                    IdActe = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CautionnementModels", x => x.IdCautionnement);
                    table.ForeignKey(
                        name: "FK_CautionnementModels_TypesActModels_IdActe",
                        column: x => x.IdActe,
                        principalTable: "TypesActModels",
                        principalColumn: "IdActe",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChargeModels",
                columns: table => new
                {
                    IdCharge = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TypeCharge = table.Column<string>(type: "text", nullable: false),
                    MontantCharge = table.Column<decimal>(type: "numeric", nullable: false),
                    DateCharge = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PayePar = table.Column<string>(type: "text", nullable: false),
                    StatutCharge = table.Column<string>(type: "text", nullable: false),
                    IdActe = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChargeModels", x => x.IdCharge);
                    table.ForeignKey(
                        name: "FK_ChargeModels_TypesActModels_IdActe",
                        column: x => x.IdActe,
                        principalTable: "TypesActModels",
                        principalColumn: "IdActe",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientActModels",
                columns: table => new
                {
                    IdCltActe = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DescriptionRole = table.Column<string>(type: "text", nullable: false),
                    IdClt = table.Column<int>(type: "integer", nullable: false),
                    IdActe = table.Column<int>(type: "integer", nullable: false),
                    IdRole = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientActModels", x => x.IdCltActe);
                    table.ForeignKey(
                        name: "FK_ClientActModels_ClientModels_IdClt",
                        column: x => x.IdClt,
                        principalTable: "ClientModels",
                        principalColumn: "IdClt",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientActModels_RolesClientActModels_IdRole",
                        column: x => x.IdRole,
                        principalTable: "RolesClientActModels",
                        principalColumn: "IdRole",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClientActModels_TypesActModels_IdActe",
                        column: x => x.IdActe,
                        principalTable: "TypesActModels",
                        principalColumn: "IdActe",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntiteJurModels",
                columns: table => new
                {
                    IdEntiteJur = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DateActeJuridique = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Reference = table.Column<string>(type: "text", nullable: false),
                    NomHuissier = table.Column<string>(type: "text", nullable: false),
                    VilleHuissier = table.Column<string>(type: "text", nullable: false),
                    NumeroDossier = table.Column<string>(type: "text", nullable: false),
                    Responsable = table.Column<string>(type: "text", nullable: false),
                    FonctionResponsable = table.Column<string>(type: "text", nullable: false),
                    NomDossier = table.Column<string>(type: "text", nullable: false),
                    IdActe = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntiteJurModels", x => x.IdEntiteJur);
                    table.ForeignKey(
                        name: "FK_EntiteJurModels_TypesActModels_IdActe",
                        column: x => x.IdActe,
                        principalTable: "TypesActModels",
                        principalColumn: "IdActe",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JournalisationModels",
                columns: table => new
                {
                    IdNotif = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TypeNotif = table.Column<string>(type: "text", nullable: false),
                    DateNotif = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ContenuNotif = table.Column<string>(type: "text", nullable: false),
                    ModeNotif = table.Column<string>(type: "text", nullable: false),
                    Destinataire = table.Column<string>(type: "text", nullable: false),
                    StatutNotif = table.Column<string>(type: "text", nullable: false),
                    TypeAnnexe = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Fichier = table.Column<string>(type: "text", nullable: false),
                    DateUpload = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UploadedBy = table.Column<string>(type: "text", nullable: false),
                    RefPret = table.Column<string>(type: "text", nullable: false),
                    Dossier = table.Column<string>(type: "text", nullable: false),
                    Cheminfichier = table.Column<string>(type: "text", nullable: false),
                    Etat = table.Column<string>(type: "text", nullable: false),
                    DateArchive = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IdActe = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalisationModels", x => x.IdNotif);
                    table.ForeignKey(
                        name: "FK_JournalisationModels_TypesActModels_IdActe",
                        column: x => x.IdActe,
                        principalTable: "TypesActModels",
                        principalColumn: "IdActe",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PretModels",
                columns: table => new
                {
                    IdPret = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReferenceConvention = table.Column<string>(type: "text", nullable: false),
                    MontantConcours = table.Column<decimal>(type: "numeric", nullable: false),
                    Taux = table.Column<decimal>(type: "numeric", nullable: false),
                    MontantLettrePret = table.Column<string>(type: "text", nullable: false),
                    DateSignaturePret = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DureMoisPret = table.Column<int>(type: "integer", nullable: false),
                    DateDebutRemboursemnt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DatePremiereEcheance = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MontantPremiereEcheance = table.Column<decimal>(type: "numeric", nullable: false),
                    DateDerniereEcheance = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MontantDerniereEcheance = table.Column<decimal>(type: "numeric", nullable: false),
                    Accessoires = table.Column<string>(type: "text", nullable: false),
                    ObjetPret = table.Column<string>(type: "text", nullable: false),
                    EtatPret = table.Column<string>(type: "text", nullable: false),
                    IdActe = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PretModels", x => x.IdPret);
                    table.ForeignKey(
                        name: "FK_PretModels_TypesActModels_IdActe",
                        column: x => x.IdActe,
                        principalTable: "TypesActModels",
                        principalColumn: "IdActe",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AutorisationModels_IdActe",
                table: "AutorisationModels",
                column: "IdActe",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CautionnementModels_IdActe",
                table: "CautionnementModels",
                column: "IdActe",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChargeModels_IdActe",
                table: "ChargeModels",
                column: "IdActe",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientActModels_IdActe",
                table: "ClientActModels",
                column: "IdActe");

            migrationBuilder.CreateIndex(
                name: "IX_ClientActModels_IdClt",
                table: "ClientActModels",
                column: "IdClt");

            migrationBuilder.CreateIndex(
                name: "IX_ClientActModels_IdRole",
                table: "ClientActModels",
                column: "IdRole");

            migrationBuilder.CreateIndex(
                name: "IX_ClientModels_NomRaisonsociale",
                table: "ClientModels",
                column: "NomRaisonsociale");

            migrationBuilder.CreateIndex(
                name: "IX_EntiteJurModels_IdActe",
                table: "EntiteJurModels",
                column: "IdActe",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JournalisationModels_IdActe",
                table: "JournalisationModels",
                column: "IdActe");

            migrationBuilder.CreateIndex(
                name: "IX_PretModels_IdActe",
                table: "PretModels",
                column: "IdActe",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TypesActModels_IdActe",
                table: "TypesActModels",
                column: "IdActe");

            migrationBuilder.CreateIndex(
                name: "IX_TypesActModels_IdAjout",
                table: "TypesActModels",
                column: "IdAjout");

            migrationBuilder.CreateIndex(
                name: "IX_TypesActModels_IdBnq",
                table: "TypesActModels",
                column: "IdBnq");

            migrationBuilder.CreateIndex(
                name: "IX_TypesActModels_IdUser",
                table: "TypesActModels",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_UserModel_NameUser",
                table: "UserModel",
                column: "NameUser");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AutorisationModels");

            migrationBuilder.DropTable(
                name: "CautionnementModels");

            migrationBuilder.DropTable(
                name: "ChargeModels");

            migrationBuilder.DropTable(
                name: "ClientActModels");

            migrationBuilder.DropTable(
                name: "EntiteJurModels");

            migrationBuilder.DropTable(
                name: "JournalisationModels");

            migrationBuilder.DropTable(
                name: "PretModels");

            migrationBuilder.DropTable(
                name: "ClientModels");

            migrationBuilder.DropTable(
                name: "RolesClientActModels");

            migrationBuilder.DropTable(
                name: "TypesActModels");

            migrationBuilder.DropTable(
                name: "AjoutActModels");

            migrationBuilder.DropTable(
                name: "BanqueModels");

            migrationBuilder.DropTable(
                name: "UserModel");
        }
    }
}
