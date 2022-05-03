select version();
select current_database();

/* All tables */
SELECT * FROM pg_catalog.pg_tables;

/* user created tables */
SELECT * FROM pg_catalog.pg_tables WHERE schemaname != 'pg_catalog' AND schemaname != 'information_schema'
