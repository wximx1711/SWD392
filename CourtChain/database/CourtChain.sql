-- CourtChain | Ho Tuan Minh | HE191253 | SE1990NJ
-- SQLite: run once against an empty database. Backend seeds the owner from environment.
PRAGMA foreign_keys = ON;
CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" TEXT NOT NULL CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY,
    "ProductVersion" TEXT NOT NULL
);

BEGIN TRANSACTION;
CREATE TABLE "Audits" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Audits" PRIMARY KEY AUTOINCREMENT,
    "Entity" TEXT NOT NULL,
    "Action" TEXT NOT NULL,
    "EntityId" INTEGER NOT NULL,
    "ActorId" INTEGER NOT NULL,
    "At" TEXT NOT NULL,
    "Before" TEXT NULL,
    "After" TEXT NOT NULL
);

CREATE TABLE "Facilities" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Facilities" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "Address" TEXT NOT NULL,
    "CityDistrict" TEXT NOT NULL,
    "Phone" TEXT NOT NULL,
    "Email" TEXT NOT NULL,
    "Opening" TEXT NOT NULL,
    "Closing" TEXT NOT NULL,
    "SlotMinutes" INTEGER NOT NULL,
    "Parking" INTEGER NOT NULL,
    "Shower" INTEGER NOT NULL,
    "ProShop" INTEGER NOT NULL,
    "Wifi" INTEGER NOT NULL,
    "Latitude" TEXT NULL,
    "Longitude" TEXT NULL,
    "Status" TEXT NOT NULL,
    "UpdatedAt" TEXT NOT NULL
);

CREATE TABLE "Users" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Users" PRIMARY KEY AUTOINCREMENT,
    "Email" TEXT NOT NULL,
    "Role" TEXT NOT NULL,
    "Salt" TEXT NOT NULL,
    "Hash" TEXT NOT NULL
);

CREATE TABLE "Courts" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Courts" PRIMARY KEY AUTOINCREMENT,
    "FacilityId" INTEGER NOT NULL,
    "Name" TEXT NOT NULL,
    "Sport" TEXT NOT NULL,
    "Surface" TEXT NOT NULL,
    "Indoor" INTEGER NOT NULL,
    "Notes" TEXT NULL,
    "Lighting" INTEGER NOT NULL,
    "Status" TEXT NOT NULL,
    "UpdatedAt" TEXT NOT NULL,
    CONSTRAINT "FK_Courts_Facilities_FacilityId" FOREIGN KEY ("FacilityId") REFERENCES "Facilities" ("Id") ON DELETE RESTRICT
);

CREATE TABLE "Policies" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Policies" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "Scope" TEXT NOT NULL,
    "FacilityId" INTEGER NULL,
    "Sport" TEXT NOT NULL,
    "Regular" TEXT NOT NULL,
    "Peak" TEXT NOT NULL,
    "Weekend" TEXT NOT NULL,
    "Discount" TEXT NOT NULL,
    "PeakStart" TEXT NOT NULL,
    "PeakEnd" TEXT NOT NULL,
    "EffectiveStart" TEXT NOT NULL,
    "EffectiveEnd" TEXT NULL,
    "Status" TEXT NOT NULL,
    CONSTRAINT "FK_Policies_Facilities_FacilityId" FOREIGN KEY ("FacilityId") REFERENCES "Facilities" ("Id") ON DELETE RESTRICT
);

CREATE UNIQUE INDEX "IX_Courts_FacilityId_Name" ON "Courts" ("FacilityId", "Name");

CREATE UNIQUE INDEX "IX_Facilities_Name" ON "Facilities" ("Name");

CREATE INDEX "IX_Policies_FacilityId" ON "Policies" ("FacilityId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20261109031728_InitialCreate', '10.0.9');

COMMIT;

BEGIN TRANSACTION;
ALTER TABLE "Facilities" ADD "ImageUrl" TEXT NULL;

ALTER TABLE "Facilities" ADD "Version" INTEGER NOT NULL DEFAULT 0;

ALTER TABLE "Courts" ADD "Version" INTEGER NOT NULL DEFAULT 0;

ALTER TABLE "Audits" ADD "Rationale" TEXT NOT NULL DEFAULT '';

CREATE INDEX "IX_Policies_Scope_FacilityId_Sport_EffectiveStart" ON "Policies" ("Scope", "FacilityId", "Sport", "EffectiveStart");

CREATE INDEX "IX_Audits_Entity_EntityId_At" ON "Audits" ("Entity", "EntityId", "At");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20261109042008_AdministrationRevisions', '10.0.9');

COMMIT;


