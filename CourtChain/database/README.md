# SQLite schema

`CourtChain.sql` creates the database schema and EF migration history. Run it once on an empty SQLite database; it is not a SQL Server script. No account password is included. The backend seeds the owner using COURTCHAIN_OWNER_PASSWORD on first startup.

Normally no manual SQL import is needed: start the backend and EF applies the migrations automatically.
