# Lycoperdon

## Humanized drum pattern player

A simple CLI utility for humanized MIDI drum pattern playback. Write simple drum beats in ASCII notation and write instructions to play those beats in a YAML-file and run.

### Features

- Customize tempo, add swing (random tempo variation), humanization (random delay in each note) and variance (random volume decrease) on the fly with commands, or specify them at song level.
- Change accents and first notes (usually a cymbal played at the start of each beat).
- Write custom patterns in song file, consisting of sub-commands that are run when the pattern is played.
- Includes some extra options for randomized drums, like `RandomCymbal`.

Lycoperdon leverages [managed-midi](https://github.com/atsushieno/managed-midi) for cross-platform MIDI playback. All drum patterns and songs are stored as YAML, with serialization powered by [SharpYaml](https://github.com/xoofx/SharpYaml).

### Syntax

Drum beats have four different volume levels, denoted by these characters:

```
- 0
. 80
+ 90
x 100
X 127
```

An example beat. See Drum.cs for all available drum options.

```
Accent:  x-+-
Beat:
  Kick:  x---
  Snare: --x-
```

Available song/pattern commands

```
- SET TEMPO xx
- SET SWING xx
- SET HUMANIZE xx
- SET VARIANCE xx

- SET FIRST drum_name
- SET ACCENT drum_name

- PLAY xx beat_name
- PLAY xx pattern_name

- SILENCE xx
```

### Running and usage

See the beats folder and the songs folder for examples.

Just clone the repository, install dotnet core 3.1 and run.

```
dotnet run SONG_NAME
dotnet run SONG_NAME MIDI_DEVICE_NUMBER
```
