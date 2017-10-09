CREATE TABLE vacancy.stored_content
(
   id 		SERIAL PRIMARY KEY, 
   vacancy_id	BIGINT,
   vacancy_data	jsonb, 
   insert_ts 	timestamp without time zone DEFAULT NOW()
) 
WITH (
  OIDS = FALSE
)
;
ALTER TABLE vacancy.stored_content
  OWNER TO test_user;

DROP TABLE vacancy.stored_content_temp

CREATE UNLOGGED TABLE vacancy.stored_content_temp
(
   vacancy_id	BIGINT,
   vacancy_data	jsonb 
) 
WITH (
  OIDS = FALSE
)
;
ALTER TABLE vacancy.stored_content
  OWNER TO test_user;
