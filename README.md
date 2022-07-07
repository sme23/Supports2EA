# Supports2EA

This is a command line tool for converting given data representing FE8 supports
and converts them into an Event Assembler .event file, for the purpose of
automating the tracking of all sorts of data annoying to track manually
when setting up support data.

## Usage

```
supports2ea "input.txt" "characters.txt"
```

`input.txt` contains the input supports script.
`characters.txt` contains character-relevant data automatically enumerated in the output file. 

The format of each line of a support script is as follows:

```
Character + Character {initialValue, growthValue}
```

Where `Character` is the name of a character defined in `characters.txt`, `initialValue` is the starting support level from 0-255, and `growthValue` is the number of support points gained each turn the units are adjacent from 0-255.

The format of `characters.txt` is the same as EA definitions, so you can reuse a character definitions file used by an EA buildfile for enumeration here.
