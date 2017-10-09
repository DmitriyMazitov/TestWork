CREATE OR REPLACE FUNCTION vacancy.fn_merge_content()
RETURNS void AS $$
BEGIN
-- insert
INSERT INTO vacancy.stored_content(vacancy_id, vacancy_data)
select src.vacancy_id, src.vacancy_data from vacancy.stored_content_temp src
WHERE src.vacancy_id NOT IN (select dest.vacancy_id from vacancy.stored_content dest);

-- update
UPDATE vacancy.stored_content u 
SET vacancy_data = p.vacancy_data
FROM (
select src.vacancy_id, src.vacancy_data from vacancy.stored_content_temp src,
vacancy.stored_content dest 
WHERE src.vacancy_id = dest.vacancy_id AND src.vacancy_data <> dest.vacancy_data) p
WHERE u.vacancy_id = p.vacancy_id;

-- delete
DELETE FROm vacancy.stored_content
WHERE vacancy.stored_content.vacancy_id IN (
select dest.vacancy_id from vacancy.stored_content dest
WHERE dest.vacancy_data NOT IN (select src.vacancy_data from vacancy.stored_content_temp src));

TRUNCATE TABLE vacancy.stored_content_temp;

END;
$$ LANGUAGE plpgsql;

