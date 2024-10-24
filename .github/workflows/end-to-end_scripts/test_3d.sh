#!/bin/bash

export DISPLAY=:99

url_3d=$1
username=$2
password=$3
coursename=$4
spacename=$5
elementname=$6

echo "url_3d: $url_3d"
echo "coursename: $coursename"
echo "username: $username"
echo "password: $password"
echo "spacename: $spacename"
echo "elementname: $elementname"

python3 $script_test_3d_py "$url_3d" "$username" "$password" "$coursename" "$spacename" "$elementname"
