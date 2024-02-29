# Changelog

## Unreleased

<details>
<summary> Changes that are not yet included in a release. </summary>

### Added

- Import and archive functionality:
  - You can now archive your learning worlds as a .zip archive which you can store elsewhere for backup purposes or
  share with others. It contains all the data necessary to restore the entire world on another machine.
  - You can now import learning worlds from .zip archives.
- New list of previously uploaded learning worlds that appears after logging in to the LMS.
- Previously uploaded worlds can now be deleted from the LMS and the AdLerBackend.
- Before a learning world is uploaded, the system now checks whether a learning world with the same name already exists on the backend. If there is a duplicate, the author can decide whether to replace the learning world or create a copy.
- Authors can now assign an enrolment key for their learning world.
- Learning outcomes can now be created for learning spaces using an input form.
- A new button in the header bar shows an overview of all learning outcomes in the selected world.

#### Story elements
- You can now create story elements: Story elements are a new type of learning element that can be used to create a story in the learning world.
- They can only contain text blocks as content.
- They can only be placed in thew new story slots in learning spaces.
- There is one story slot at the entrance and one at the exit of each learning space.
- The story element at the entrance of a space will be triggered when the user first enters the space, and the story element at the exit will be triggered when the user leaves the space.
- Story elements are represented in the 3D world as NPCs that the user can interact with.
- Story elements cannot grant points or be required to complete a space.
- Story elements cannot have a difficulty, estimated workload or goals (learning outcomes).
- The content of a story element cannot be created in advance (just like the adaptivity element and unlike normal learning elements).
- As such, the content of story elements will not be listed in the external learning material overview.

### Changed

- Presentation of the learning elements in the hierarchy and in Moodle according to the linear order of the floor plan.
- The Create-World, -Space, -Element and -Link dialogs can be confirmed with the Enter key.
- Changed the way learning worlds are fetched internally and displayed on the start screen.
    - You can now see the last modify time and optionally file name of worlds.
    - You can now sort worlds by name or last modify time.
- UI:
  - Optimized styling of the adaptivity dialog.
  - Optimized responsive design.
  - Reworked learning world tree view.
    - Selecting a learning space while tree view is open now shows the learning space in blue in the tree view.
    - Selecting a learning element while tree view is open now opens the learning space in the tree view (if not open yet) and shows the learning element in blue in the tree view.
    - Creating a new learning space while tree view is open now also shows the learning space in blue in the tree view.
    - Creating a new learning element inside a learning space (by clicking the "+" button on an empty slow) now also shows the learning element in blue in the tree view.
  - The container for the unplaced learning elements is now in the bottom center below the space view.
  - Separate sidebar panel for different types of learning elements

### Deprecated

-

### Removed

- Removed right click menu entry "Show" for adaptivity elements.

### Fixed

- Fixed bug where world has no unsaved changes when editing the content of a adaptivity element.

### Security

-

</details>

## [2.0.0] - 2023-11-17

### Added

- Adaptivity element:
    - New specific icon for adaptivity element
    - New selectable 3D models
    - Dialogs for creating tasks, single- and multiple response questions with selectable level of difficulty
    - Tasks can require a certain level of difficulty or be optional
    - Option to add comments and learning element references to questions
- Tooltips added to many UI elements
- Learning spaces and conditions are now created among each other in path way view

### Fixed

- Fixed pop up of a second dialog when deleting learning content is cancelled

### Changed

- Improved responsive design
- Reworked presentation of learning element models
- except for the adaptivity element, any 3D model can now be selected for any element
- Changes in editing dialogs are now automatically applied
- Many UI elements redesigned
- When saving a learning world, the user is no longer prompted for the save path - it is saved to an internal folder
  instead.
- When deleting learning paths, the selected path is now preferred
- New icons and colors for the difficulty of learning elements
- Reworked upload learning world dialog
- Learning elements are now arranged in the direction of movement through the room

## [2.0.0-rc.4] - 2023-11-9

## [2.0.0-rc.3] - 2023-11-8

## [2.0.0-rc.2] - 2023-11-7

## [2.0.0-rc.1] - 2023-10-30

## [1.1.0] - 2023-09-12

**Full Changelog**: https://github.com/ProjektAdLer/Autorentool/compare/1.0.0...1.1.0

## What's Changed

* Introduced the ability to set the link to a user evaluation which will be presented to students after completing the
  learning world in the AdLer learning environment.
* Introduced a table similar to the table in the content files view when creating or editing learning elements to make
  selecting them easier.
* Defined a new ordering algorithm to make display of learning spaces in the world views more consistent.
* Improved logging and error management capabilities.
* Generally improved UI and usability.
* Fixed various bugs.

## [1.0.3] - 2023-08-22

**Full Changelog**: https://github.com/ProjektAdLer/Autorentool/compare/1.0.2...1.0.3

- Fixed a bug where a saved token but empty URL in a config file crashed the program when opening the login dialog
- Fixed a bug where the API being unreachable via network crashed the program when opening the login dialog
- Fixed a bug where a saved token being reported as invalid by the API crashed the program when opening the login dialog
- Fixed a bug where learning pathways couldn't be deleted after loading a saved world file
- Fixed a bug where it was impossible to change the content of an existing learning element from a content that
  references a file to a content that references a link and vice versa

## [v1.0.2] - 2023-06-26

**Full Changelog**: https://github.com/ProjektAdLer/Autorentool/compare/1.0.1...1.0.2

- Fixed bug where we generated an invalid Moodle Backup File.

## [v1.0.1] - 2023-06-22

**Full Changelog**: https://github.com/ProjektAdLer/Autorentool/compare/1.0.0...1.0.1

- Fixed a critical bug that prevented changing the learning content of an element from one content of a type to another
  content of that type (e.g. text content -> text content)

## [v1.0.0] - 2023-06-19

- The software now features a complete redesign and restructuring of all components to incorporate a new layout.

- Dialogues have been improved to no longer block the operation of the remaining components and can now be opened via
  icons in a new sidebar.

- A new feedback button has been added in the header bar for users to communicate their experiences and report any
  discovered bugs.

- The application now supports switching the language interface between German and English.

- Authors now have the ability to log into the Learning Management System (LMS), Moodle, with backend-URL, username, and
  password.

- The software has implemented a feature to track unsaved changes within the world.

- All loaded and saved learning worlds are now conveniently displayed in a grid view upon launching the authoring tool,
  and these can be managed (opened or deleted) with a single click.

- Both the "Description" and "Goals" fields in the Learning Worlds and Learning Spaces have been enlarged, allowing for
  more information to be added, including the addition of multiple goals using line breaks.

- In Learning Spaces, the "Shortname" and "Authors" fields have been removed, and users now have three selectable space
  layouts with a fixed number of slots.

- A new field, "Theme," has been introduced for users to select the design of the Space, with "Campus" being the current
  available option.

- To enhance the learning path creation, "And" and "Or" conditions can now be switched between spaces. This allows users
  to represent complex learning paths and create these paths in a new component where learning rooms and conditions can
  be connected.

- Learning Elements have undergone significant changes. The "Shortname", "Authors", "Url", and "Type" fields have been
  removed. Elements can now be created either directly in spaces' slots or as unplaced elements in the world, allowing
  for better organization.

- Learning content is now loaded independently from the creation of learning elements. Furthermore, users can move these
  elements between slots or the list of unplaced elements via drag & drop.

- The new "All Files" component allows users to load learning content into the authoring tool independently from the
  elements. This component supports simultaneous loading of multiple learning contents which remain available even after
  closing and reopening the tool and can be used across different worlds.

- Another addition is the "LearningWorldOverview" component that displays all created spaces and elements within the
  world, enhancing navigation between these.

- Before exporting the learning world, a new validation process ensures everything is in order.

- The software has now been connected to the new Moodle plugin and supports the new ATF 0.4.

- Through logging into Moodle, authors can now directly upload their learning worlds as a course.

## [v0.3.2] - 2022-11-21

**Full Changelog**: https://github.com/ProjektAdLer/Autorentool/compare/0.3.1...0.3.2

This release fixes several critical bugs that made the application hard to use.

## [v0.3.1] - 2022-10-28

**Full Changelog**: https://github.com/ProjektAdLer/Autorentool/compare/0.3.0...0.3.1

This minor release includes a fix for our backup generation format.

## [v0.3.0] - 2022-10-18

**Full Changelog**: https://github.com/ProjektAdLer/Autorentool/compare/0.2.0...0.3.0

## Major Features

- LearningPathways implemented.
- All currently supported content types are now exported to Moodle.
- Switched to a command based architecture internally and implemented the Memento pattern to properly save versions of
  Worlds, Spaces and Elements for Undo and Redo.
- Implemented Undo and Redo on (most) UI commands.
- Switched internally from custom mapper implementations to using AutoMapper.
- Implemented text based elements.
- Implemented points on both LearningElements (points on completion) and LearningSpaces (required points to complete
  space).
- Removed LearningElements from World directly, they can now only be created in spaces. (If you want an element in the
  world, just make a space with one element.)
- Switched video based elements to use a URL instead of a file (for filesizes sake).
- Changed serialization format for saving Worlds, Spaces and Elements.
- Instead of loading learning content, we now save it into a special folder and use filepaths.
- Implemented a manager class for this purpose.
- Updated DSL format.
- Revisited which fields are required or optional when creating Worlds, Spaces and Elements.
- Marked required fields with a star.
- Added some validation when creating Worlds, Spaces and Elements.

## [v0.2.0] - 2022-06-29

The following new Features have been implemented:

### Toolbox

- A Toolbox component has been implemented.
- The toolbox can contain Learning worlds, spaces and elements.
- Objects in this toolbox are loaded from a special system folder (e.g. ??? on Windows, /home/[user]
  /.config/AdLerAuthoring/Toolbox on Linux).
- Double clicking on a world loads it into the world selection dropdown.
- Double clicking on a space inserts it into the currently focused learning world.
- Double clicking on an element either inserts it into the currently focused learning world, or, if applicable, into the
  currently opened learning space.
- Searching in the toolbox filters objects and only displays objects which contain the search string.
- If the search string contains the special terms "world", "space" or "element", only elements of that type are
  displayed. If the term is followed by a colon (':') and another search term, only elements of that type that also
  contain the search term in the name are displayed.
- If the search term is surrounded by quotation marks, the term is taken literally and the rules above do not apply,
  instead elements are filtered only by name.

### Learning elements

- There are now tighter restrictions on the combinations of type and content that are selectable when creating new
  learning elements or editing existing ones.
- The type system under the hood has been refactored to reflect these restrictions.
- When creating learning elements, you must now supply a file containing the content of the learning element.
- Only files corresponding to the selected type-content combination can be selected (e.g. .h5p files for H5P elements).
- You can now enter an estimated workload and a difficulty on learning elements.
    - For spaces and worlds, the sum of estimated workloads is calculated from the elements and displayed accordingly.

### User Interface

- You can now add learning elements to worlds and spaces more quickly by simply dragging the desired content file onto
  the drop zone.
    - A new create element dialog will appear with some prefilled fields.
- You can also drop AdLer world, space and element files onto the drop zone to open or add them to the world/space,
  respectively.

### Moodle Export

- It is now possible to export learning worlds with any number of H5P elements into a Moodle backup.
- The backup will now also contain a DSL file, describing the learning world, which is required for the 3D AdLer engine
  to properly display said world.
- Other elements types other than H5P are unfortunately not yet supported, but will be in future releases.

### Other

- General bugfixes and improvements

## [v0.1.0] - 2022-04-05

This first prerelease consists of the following features:

- Create, Delete, Edit, Save and Load learning worlds, elements and spaces!
- Elements and spaces can currently be placed in worlds, placing elements inside of spaces will be implemented in the
  next version.
- Generation of an empty but valid Moodle backup file without use of templates or other temp files.

Known bugs and issues:

- Pressing cancel on the Save or Load dialogues crashes the application
- Loaded worlds/spaces/elements sometimes don't show up immediately and a new object of the same type must be created
  before it shows
- Startup times can vary widely

Installation:
Windows: Please use either the Setup .exe or unpack `win-unpacked` into a folder of choice, then start AuthoringTool.exe
Linux: It is highly recommended to unpack `linux-unpacked` into a folder of your choice and then run the
file `authoring-tool` manually, as this method is by *FAR* faster than the AppImage or snap packages, which are also
provided for your convenience.
MacOS: Either use the .dmg file or the provided `-mac.zip` below.
