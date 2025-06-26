# MtarTool
.mtar (Motion Archive) unpacker and repacker for Pro Evolution Soccer 2017+ series.

Allows you to extract PES's animation files (.gani) from the motion archive containers.

## Usage:

Drag a .mtar file onto the tool. If it is an Mtar Type 1 file, the tool will simply extract the .gani files from it.

Drag a .xml file produced by the tool onto the tool to repack the .mtar.

The tool will generate a hashed_names.txt file containing any unhashed name's hashed equivalent for tracking purposes.

Set the -n parameter to add a four digit id to the beginning of every file's name. The number keeps track of the order the files were stored in the .mtar.

**Usage Example:**

> MtarTool.exe Example.mtar -n

## File Format Descriptions:

**.gani**: PES's animation format.

## Credits:

BobDoleOwndU - Original MTARTool, QuickHash algro

kapuragu - Fox Engine Templates
