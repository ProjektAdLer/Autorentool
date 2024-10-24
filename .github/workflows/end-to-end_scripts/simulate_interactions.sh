#!/bin/bash

# Laden Sie die FIND_ELEMENT Funktion
source "$script_define_test_script_sh"

backendurl=$1
coursename=$2
username=$3
password=$4
spacename=$5
elementname=$6

echo "backendurl: $backendurl"
echo "coursename: $coursename"
echo "username: $username"
echo "password: $password"
echo "spacename: $spacename"
echo "elementname: $elementname"


# echo Click on import-world-button
# coords=($(FIND_ELEMENT "class" "import-world-button"))
# DISPLAY=:99 xdotool mousemove ${coords[0]} ${coords[1]} sleep 0.5 click 1 sleep 1
#
# DISPLAY=:99 xdotool sleep 1
# DISPLAY=:99 xdotool key "downarrow" key "downarrow" key "Return" sleep 2
# DISPLAY=:99 xdotool key "ctrl+l" sleep 1
# DISPLAY=:99 xdotool type "$GITHUB_WORKSPACE/.github/workflows"
# DISPLAY=:99 xdotool key Return
#
# # Wählen Sie eine Datei aus und öffnen Sie sie
# DISPLAY=:99 xdotool type "testWorld.zip"
# sleep 1
# DISPLAY=:99 xdotool key "Return"
# sleep 2

 
echo Click on create-world-button
coords=($(FIND_ELEMENT "class" "create-world-button"))
DISPLAY=:99 xdotool mousemove ${coords[0]} ${coords[1]} sleep 0.1 click 1 sleep 0.1
DISPLAY=:99 xdotool type $coursename
DISPLAY=:99 xdotool sleep 0.1 key "Tab"
DISPLAY=:99 xdotool type "shortName"
DISPLAY=:99 xdotool sleep 0.1 key "Return"

echo Click on space-metadata-icon
coords=($(FIND_ELEMENT "src" "space-metadata-icon.png"))
DISPLAY=:99 xdotool mousemove ${coords[0]} ${coords[1]} sleep 0.1 click 1 sleep 0.1
DISPLAY=:99 xdotool type $spacename
DISPLAY=:99 xdotool sleep 0.1 key "Return" sleep 0.1

echo Click on slot ele_0
coords=($(FIND_ELEMENT "identifier" "ele_0"))
DISPLAY=:99 xdotool mousemove ${coords[0]} ${coords[1]} sleep 0.1 click 1 sleep 0.1

echo Click on adaptivityelement-icon
coords=($(FIND_ELEMENT "src" "adaptivityelement-icon.png"))
DISPLAY=:99 xdotool mousemove ${coords[0]} ${coords[1]} sleep 0.1 click 1 sleep 0.1
DISPLAY=:99 xdotool type $elementname
DISPLAY=:99 xdotool sleep 0.1 key "Return" sleep 0.1

echo Click on add-tasks
coords=($(FIND_ELEMENT "class" "add-tasks"))
DISPLAY=:99 xdotool mousemove ${coords[0]} ${coords[1]} sleep 0.1 click 1 sleep 0.1

echo Click on Neue Aufgabe erstellen
coords=($(FIND_ELEMENT "title" "Aufgabe erstellen"))
DISPLAY=:99 xdotool mousemove ${coords[0]} ${coords[1]} sleep 0.1 click 1 sleep 0.1

echo Neue Aufgabe umbenennen
coords=($(FIND_ELEMENT "type" "text"))
DISPLAY=:99 xdotool mousemove ${coords[0]} ${coords[1]} sleep 0.1 click 1
DISPLAY=:99 xdotool sleep 0.1 key "ctrl+a" key "BackSpace" sleep 0.1
DISPLAY=:99 xdotool type "testAufgabe"
sleep 0.1

echo Click on Mittelschwer
coords=($(FIND_ELEMENT "buttontext" "Mittelschwer"))
DISPLAY=:99 xdotool mousemove ${coords[0]} ${coords[1]} sleep 0.1 click 1 sleep 0.1

echo Click on mud-input-root-outlined
coords=($(FIND_ELEMENT "class" "mud-switch-button"))
coords[1]=$((coords[1] - 70))
DISPLAY=:99 xdotool mousemove ${coords[0]} ${coords[1]} sleep 0.1 click 1
DISPLAY=:99 xdotool type "Macht es Spass, Pipelines zu erstellen?"
sleep 0.1

echo Click on switch-button
coords=($(FIND_ELEMENT "class" "mud-switch-button"))
DISPLAY=:99 xdotool mousemove ${coords[0]} ${coords[1]} sleep 0.1 click 1 sleep 0.1

echo first radio button
coords=($(FIND_ELEMENT "type" "radio" 0))
coords[0]=$((coords[0] + 60))
DISPLAY=:99 xdotool mousemove ${coords[0]} ${coords[1]} sleep 0.1 click 1
DISPLAY=:99 xdotool sleep 0.1 key "ctrl+a" key "BackSpace" sleep 0.1
DISPLAY=:99 xdotool type "Ja"

echo second radio button
coords=($(FIND_ELEMENT "type" "radio" 1))
coords[0]=$((coords[0] + 60))
DISPLAY=:99 xdotool mousemove ${coords[0]} ${coords[1]} sleep 0.1 click 1
DISPLAY=:99 xdotool sleep 0.1 key "ctrl+a" key "BackSpace" sleep 0.1
DISPLAY=:99 xdotool type "Nein"
coords[0]=$((coords[0] - 60))
DISPLAY=:99 xdotool mousemove ${coords[0]} ${coords[1]} sleep 0.1 click 1 sleep 0.1

echo Click on add answer
coords=($(FIND_ELEMENT "class" "mud-button-icon-size-small"))
DISPLAY=:99 xdotool mousemove ${coords[0]} ${coords[1]} sleep 0.1 click 1 sleep 0.1

echo third radio button
coords=($(FIND_ELEMENT "type" "radio" 2))
coords[0]=$((coords[0] + 60))
DISPLAY=:99 xdotool mousemove ${coords[0]} ${coords[1]} sleep 0.1 click 1
DISPLAY=:99 xdotool sleep 0.1 key "ctrl+a" key "BackSpace" sleep 0.1
DISPLAY=:99 xdotool type "Auf jeden Fall!!"
coords[0]=$((coords[0] - 60))
DISPLAY=:99 xdotool mousemove ${coords[0]} ${coords[1]} sleep 0.1 click 1 sleep 0.1

echo Click on Erstellen
coords=($(FIND_ELEMENT "buttontext" "Erstellen"))
DISPLAY=:99 xdotool mousemove ${coords[0]} ${coords[1]} sleep 0.1 click 1 sleep 0.1

echo Click on mud-button-close
coords=($(FIND_ELEMENT "class" "mud-button-close"))
DISPLAY=:99 xdotool mousemove ${coords[0]} ${coords[1]} sleep 0.1 click 1 sleep 0.1

echo Click on mud-success-text
coords=($(FIND_ELEMENT "class" "mud-success-text"))
DISPLAY=:99 xdotool mousemove ${coords[0]} ${coords[1]} sleep 0.1 click 1 sleep 0.1

echo Click on upload .mbz
coords=($(FIND_ELEMENT "title" "(.mbz)"))
DISPLAY=:99 xdotool mousemove ${coords[0]} ${coords[1]} sleep 0.1 click 1 sleep 0.1

echo Click on mud-button-text-primary
coords=($(FIND_ELEMENT "class" "mud-button-text-primary"))
DISPLAY=:99 xdotool mousemove ${coords[0]} ${coords[1]} sleep 0.1 click 1 sleep 0.1

echo Click on Einloggen
coords=($(FIND_ELEMENT "title" "Einloggen auf AdLer-Server"))
DISPLAY=:99 xdotool mousemove ${coords[0]} ${coords[1]} sleep 0.1 click 1 sleep 0.1

echo Click on AdLer API URL
coords=($(FIND_ELEMENT "labeltext" "AdLer API URL"))
DISPLAY=:99 xdotool mousemove ${coords[0]} ${coords[1]} sleep 0.1 click 1
DISPLAY=:99 xdotool type "${backendurl}/api"
sleep 0.1

echo Click on Benutzername
coords=($(FIND_ELEMENT "labeltext" "Benutzername"))
DISPLAY=:99 xdotool mousemove ${coords[0]} ${coords[1]} sleep 0.1 click 1
DISPLAY=:99 xdotool type $username
sleep 0.1

echo Click on Passwort
coords=($(FIND_ELEMENT "labeltext" "Passwort"))
DISPLAY=:99 xdotool mousemove ${coords[0]} ${coords[1]} sleep 0.1 click 1
DISPLAY=:99 xdotool type $password
sleep 0.1

echo Click on mud-icon-button-edge-end
coords=($(FIND_ELEMENT "class" "mud-icon-button-edge-end"))
DISPLAY=:99 xdotool mousemove ${coords[0]} ${coords[1]} sleep 0.1 click 1 sleep 0.1

echo Click on Anmelden
coords=($(FIND_ELEMENT "buttontext" "Anmelden"))
DISPLAY=:99 xdotool mousemove ${coords[0]} ${coords[1]} sleep 0.1 click 1 sleep 0.1

echo Refresh
DISPLAY=:99 xdotool sleep 0.1 key "F5" sleep 0.1 key "ctrl+r" sleep 0.1

echo Click on upload .mbz
coords=($(FIND_ELEMENT "title" "(.mbz)"))
DISPLAY=:99 xdotool mousemove ${coords[0]} ${coords[1]} sleep 0.1 click 1 sleep 0.1

echo Click on Hochladen
coords=($(FIND_ELEMENT "buttontext" "Hochladen"))
DISPLAY=:99 xdotool mousemove ${coords[0]} ${coords[1]} sleep 0.1 click 1 sleep 2

echo Click on mud-button-text-primary
coords=($(FIND_ELEMENT "class" "mud-button-text-primary"))
DISPLAY=:99 xdotool mousemove ${coords[0]} ${coords[1]} sleep 0.1 click 1 sleep 5

echo Click on Anmelden
coords=($(FIND_ELEMENT "buttontext" "Anmelden"))
DISPLAY=:99 xdotool mousemove ${coords[0]} ${coords[1]} sleep 0.1 click 1 sleep 0.1

echo "------------"  

echo "Interactions completed"
