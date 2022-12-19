# dwg2kml
Converts a AutoCAD DWG files to KML Files using AutoCAD Civil3D

For every file, this program creates a script file, that executes a MapExport command and quits AutoCAD again.
As this is a long lasting process, it will cache a hash of processed drawings, to only convert changed files at a new run.

## Command Line Usage

--source Source directory
All drawings within that directory or subdirectories will be converted

--destination Destination folder

--profile AutoCAD MapExport profile file used for conversion

--included-drawing-regex Only process files, that match this pattern
