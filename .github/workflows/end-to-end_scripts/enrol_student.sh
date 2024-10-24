#!/bin/bash

export DISPLAY=:99

moodleurl=$1
coursename=$2
username=$3
password=$4

echo "moodleurl: $moodleurl"
echo "coursename: $coursename"
echo "username: $username"
echo "password: $password"

URL_USERNAME=$(python -c "import urllib.parse; print(urllib.parse.quote('${_USER_NAME_ARRAY_0}'))")
URL_PASSWORD=$(python -c "import urllib.parse; print(urllib.parse.quote('${_USER_PASSWORD_ARRAY_0}'))")

# Token abrufen
TEST_URL="${moodleurl}/login/token.php?username=${URL_USERNAME}&password=${URL_PASSWORD}&service=adler_services"
echo "TEST_URL: ${TEST_URL}"
TOKEN_RESPONSE=$(curl -s "${TEST_URL}")
echo "Token-Response erhalten: $TOKEN_RESPONSE"
TOKEN=$(echo $TOKEN_RESPONSE | jq -r '.token')
echo "Token erhalten: $TOKEN"

# Kurs mit Name suchen
TEST_URL="${moodleurl}/webservice/rest/server.php?wstoken=${TOKEN}&wsfunction=core_course_search_courses&criterianame=search&criteriavalue=${coursename}&moodlewsrestformat=json"
echo "TEST_URL: ${TEST_URL}"
COURSES_RESPONSE=$(curl -s "${TEST_URL}")
echo "Kurse erhalten: $COURSES_RESPONSE"

# Kurs-ID extrahieren
COURSE_ID=$(echo $COURSES_RESPONSE | jq -r '.courses[0].id')
echo "Gefundene Kurs-ID: $COURSE_ID"

# Auf Login-Seite navigieren und einloggen

python3 "$script_enrol_student_py" "$moodleurl" "$username" "$password" "$COURSE_ID"
