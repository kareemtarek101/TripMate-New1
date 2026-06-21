INSERT INTO TripTypes (name, description)
VALUES
('Adventure', 'Adventure and outdoor trips'),
('Beach', 'Beach vacations'),
('Luxury', 'Luxury travel experiences'),
('Family', 'Family friendly trips'),
('Business', 'Business travel');


SELECT
    destination_id,
    name,
    trip_type_id
FROM Destinations

UPDATE Destinations
SET trip_type_id = 1
WHERE destination_id = 2;

SELECT destination_id, name, trip_type_id
FROM Destinations

SELECT *
FROM TripTypes

SELECT trip_type_id, Name
FROM TripTypes