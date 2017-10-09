CREATE OR REPLACE VIEW vacancy.v_get_vacany_body
AS
SELECT 	sc.id
	,sc.vacancy_data::jsonb ->> 'id' AS vacancy_id
	,sc.vacancy_data::jsonb ->> 'description' AS description
	,sc.vacancy_data::jsonb -> 'schedule' ->>'name' AS schedule
	,sc.vacancy_data::jsonb ->> 'accept_kids' AS accept_kids
	,sc.vacancy_data::jsonb -> 'experience' ->> 'name' AS experience
	,sc.vacancy_data::jsonb ->> 'alternate_url' AS alternate_url
	,sc.vacancy_data::jsonb ->> 'apply_alternate_url' AS apply_alternate_url
	,sc.vacancy_data::jsonb -> 'department' ->> 'name' AS department
	,sc.vacancy_data::jsonb -> 'employment' ->> 'name' AS employment
 	,sc.vacancy_data::jsonb -> 'salary' ->> 'to' AS salary_to
 	,sc.vacancy_data::jsonb -> 'salary' ->> 'from' AS salary_from
 	,sc.vacancy_data::jsonb -> 'salary' ->> 'currency' AS salary_currency
 	,sc.vacancy_data::jsonb -> 'salary' ->> 'gross' AS salary_gross
 	,sc.vacancy_data::jsonb ->> 'archived' AS archived
 	,sc.vacancy_data::jsonb ->> 'name' AS vacancy_name
 	,sc.vacancy_data::jsonb -> 'area' ->> 'url' AS area_url
 	,sc.vacancy_data::jsonb -> 'area' ->> 'name' AS area_name
 	-- ,sc.vacancy_data::jsonb ->> 'published_at' AS published_at
 	,to_timestamp(sc.vacancy_data::jsonb ->> 'published_at','YYYY-MM-DD hh24:mi:ss')::timestamp with time zone at time zone 'Etc/UTC' AS published_at
 	,sc.vacancy_data::jsonb -> 'employer' ->> 'name' AS employer_name
 	,sc.vacancy_data::jsonb -> 'employer' ->> 'url' AS employer_url	
FROM vacancy.stored_content sc;



-- Address
CREATE OR REPLACE VIEW vacancy.v_get_vacany_address
AS
SELECT 	sc.id
	,sc.vacancy_data::jsonb -> 'address' ->> 'city' AS city
	,sc.vacancy_data::jsonb -> 'address' ->> 'street' AS street
	,sc.vacancy_data::jsonb -> 'address' ->> 'building' AS building
	,sc.vacancy_data::jsonb -> 'address' ->> 'description' AS description
	,sc.vacancy_data::jsonb -> 'address' ->> 'lat' AS lat
	,sc.vacancy_data::jsonb -> 'address' ->> 'lng' AS lng
	,sc.vacancy_data::jsonb -> 'raw' ->> 'raw' AS address_raw
FROM vacancy.stored_content sc;
--
CREATE OR REPLACE VIEW vacancy.v_get_vacany_address_metro
AS
SELECT  ms.id 
	,m.value::jsonb ->> 'station_id' AS station_id
	,m.value::jsonb ->> 'station_name' AS station_name
	,m.value::jsonb ->> 'line_id' AS line_id
	,m.value::jsonb ->> 'line_name' AS line_name
	,m.value::jsonb ->> 'lat' AS lat	
	,m.value::jsonb ->> 'lng' AS lng
FROM 
(SELECT sc.id, sc.vacancy_data::jsonb -> 'address' -> 'metro_stations' AS metro_stations_array FROM vacancy.stored_content sc) ms
,jsonb_array_elements(ms.metro_stations_array) m


-- Specialization
CREATE OR REPLACE VIEW vacancy.v_get_vacany_specialization
AS
SELECT  sp.id 
	,m.value::jsonb ->> 'profarea_id' AS specialization_profarea_id
	,m.value::jsonb ->> 'profarea_name' AS specialization_profarea_name
	,m.value::jsonb ->> 'id' AS specialization_id
	,m.value::jsonb ->> 'name' AS specialization_name
FROM 
(SELECT sc.id, sc.vacancy_data::jsonb -> 'specializations' AS specializations_array FROM vacancy.stored_content sc) sp
,jsonb_array_elements(sp.specializations_array) m
--


-- Contacts
CREATE OR REPLACE VIEW vacancy.v_get_vacany_contacts
AS
SELECT 	sc.id
	,sc.vacancy_data::jsonb -> 'contacts' ->> 'name' AS contact_name
	,sc.vacancy_data::jsonb -> 'contacts' ->> 'email' AS contact_email	
FROM vacancy.stored_content sc;
--
CREATE OR REPLACE VIEW vacancy.v_get_vacany_contacts_phone
AS
SELECT  sp.id 
	,m.value::jsonb -> 'phone' ->> 'country' AS phone_country
	,m.value::jsonb -> 'phone' ->> 'city' AS phone_city
	,m.value::jsonb -> 'phone' ->> 'number' AS phone_number
	,m.value::jsonb -> 'phone' ->> 'comment' AS phone_comment
FROM 
(SELECT sc.id, sc.vacancy_data::jsonb -> 'phone' AS phone_array FROM vacancy.stored_content sc) sp
,jsonb_array_elements(sp.phone_array) m
