# Tetris

![.NET](https://github.com/phduarte/Tetris/actions/workflows/main.yml/badge.svg)
[![CodeFactor](https://www.codefactor.io/repository/github/phduarte/tetris/badge)](https://www.codefactor.io/repository/github/phduarte/tetris)
[![Codacy Badge](https://api.codacy.com/project/badge/Grade/d567b76d931e4e619802302fdf0124b7)](https://www.codacy.com/manual/phduarte87/Tetris?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=phduarte/Tetris&amp;utm_campaign=Badge_Grade)

*[Leia em português](README.md)*

Tetris is a game released by Nintendo in 1989 and one of the most famous video games of all time.

This version was first inspired by brick games and was built only to study and practice Software Engineering disciplines.

The project uses Tetris as its setting because the game is so well known, which makes it easy to relate the real-world rules to the code written here.

## History

In 1984, at the Computing Centre of the Soviet Academy of Sciences, the Russian mathematician Alexey Pajitnov was studying a combinatorics theory about Solomon Golomb's pentominoes. Pentominoes are pieces made of five blocks that can change shape to fit together. Pajitnov created a simplified version of the theory with 4-block pieces, the tetrominoes.

In June 1984 he turned his theory into a game that quickly became very popular. It was later found by the Englishman Robert Stein, who brought it to the West, where it caught Nintendo's attention.

Over the years the distribution of the game became the subject of complex legal disputes involving three countries and the main game companies of the time. Nintendo won, and made Tetris the launch game of the Game Boy in 1989, in place of the company's main mascot, Mario.

The Game Boy version sold 70 million copies, and the game entered the Guinness Book as the most ported game in history, released on 65 different platforms.

Because it was developed at a Soviet state institution by a Soviet citizen, the rights to sell the game were held by the State for 10 years, and Alexey only started receiving royalties in 1996.

A curious fact: Steve Wozniak, co-founder of Apple, was one of the game's top record holders and used to send his scores to Nintendo Power magazine. After a while the magazine stopped accepting them, but he kept sending his records under a fake name, Evets Kainzow, which is Steve Wozniak spelled backwards.

## Architecture

The purpose of this project is to show, in a simple way, how to apply the methodologies, patterns and principles that are popular today. Every implementation follows Clean Code good practices, Design Patterns and principles such as SOLID, while trying to avoid over-engineering. So even when a pattern could be applied, its cost-benefit is still weighed. The project is always open to refactoring.

The solution started with only two packages, Core and Desktop. Through refactoring it was split into layers, on .NET 10:

```
Desktop ──► Application ──► Domain
                       ├──► Data ──► Domain
                       └──► SoundPlayer
```

| Project | Responsibility |
| --- | --- |
| `Gadz.Tetris.Domain` | Domain model and all game rules (board, pieces, collision, scoring, levels, high score and states). Depends on no other project. |
| `Gadz.Tetris.Data` | Statistics repository, saved to `%USERPROFILE%\Tetris\stats.mc`. |
| `Gadz.Tetris.SoundPlayer` | Audio adapter over `winmm.dll`. |
| `Gadz.Tetris.Application` | `GameController` (facade for the user interfaces) and the keyboard commands. Wires the domain to the infrastructure. |
| `Gadz.Tetris.Desktop` | WinForms interface: handles device input and gives visual feedback to the player. Holds no game rules. |
| `Gadz.Tetris.Tests` | NUnit tests for the domain and the `GameController`. |

While modeling the domain, technology neutrality and decoupled responsibilities were essential, so the domain has no links to the infrastructure or interface layers. After all, players must not be able to tamper with the game rules, and the game should not be tied to a single interface. There will probably be versions for other devices, such as mobile. Inside each project, whatever does not need to be seen from outside is hidden with the *internal* and *private* modifiers.

The evolution plans are in [docs/PLANO-REFATORACAO.md](docs/PLANO-REFATORACAO.md) and [docs/PLANO-MULTIPLATAFORMA.md](docs/PLANO-MULTIPLATAFORMA.md) (in Portuguese).

## Features

- **Classic theme**: a look similar to the original 1980s game.
- **Brick Game theme**: a look similar to the handheld brick games that were popular in the 1990s.
- **Slide**: holding CTRL while moving the piece left or right slides it all the way to the edge.
- **Smash Down**: pressing SPACEBAR drops the piece at once.
- **Sounds**: events and moves play sounds similar to those of brick games.
- **Sound On/Off**: turn the sound on or off on the start screen or during the game by pressing SHIFT.
- **Scoring**:
  + 1 complete line is worth 40 points
  + 2 complete lines in the same move are worth 100 points
  + 3 complete lines in the same move are worth 300 points
  + TETRIS (4 complete lines in the same move) is worth 1,200 points
- **Levels and speed**: the level goes up when you reach the line goal (20 on the first level, growing 25% each level). On level 1 the piece falls every 1,000 ms, and this interval drops 50 ms per level down to a minimum of 100 ms.
- **HI-SCORE**: the game screen shows the high score below the score. It is the highest score ever saved, and it follows the current score once the current game beats it.

## Controls

| Key | Action |
| --- | --- |
| ← / → | Move the piece |
| CTRL + ← / → | Slide the piece to the edge |
| ↓ | Move the piece down |
| ↑ | Rotate the piece |
| SPACE | Drop the piece (smash down) |
| ENTER | Pause / resume |
| ESC | End the game and save the statistics |
| SHIFT | Turn the sound on / off |

## Screenshot

![img1](docs/screenshots/tela2.1.png)

## Conclusion

When I built this game, my goal was not to look at other projects as examples. I only looked up the rules and terms. I wanted to build it entirely without technical help, believing this would keep my study more neutral, so I could reach a conclusion based on theory and my own practice, without outside influence.

Below are my conclusions for each field of study.

### Domain-Driven Design

Even though the context is fairly small, DDD helped me create a concise domain model. All development started from it: understanding the rules, who they belonged to, which entities really were part of the domain, how they related to each other, and where and when the rules should be implemented. The system grew around the domain, and that let me build it in two weeks, because at no point did I wonder whether I was doing it right or had forgotten something. The domain felt mature and supported me throughout development.

I also felt confident during development thanks to adopting the ubiquitous language from the start. It helped me understand the problem better and learn along the way; the clarity of the language let me ask better questions and feel better prepared to understand the answers.

### Windows Forms

Choosing a technology that is not very popular today was deliberate: it serves as a base for future versions with different user interfaces.

This version had to be finished quickly, and Windows Forms helped by making it easy to work with callbacks through events and delegates.

The project is light and small to download, thanks to the many libraries that come built into .NET.

### Clean Code

After finishing the first version, I found many other implementations built as simpler projects.

In general the solution logic looked similar, but some of those projects seemed harder to maintain. Some were written in a single class or method: the structure may not scare you, but opening that file does. Many overused conditionals and loops, which led to very high cyclomatic complexity. Magic numbers and comments were also things I did not want to have here.

In the end, my impression is that putting everything in a single file is easier and faster while building, while splitting it into a readable structure takes more work and time. However, that effort pays back once the project is done and goes into maintenance. So if maintaining your system matters more, or is most of the application's life cycle, a layered architecture, especially one focused on the problem domain, is usually the more efficient choice.

### SOLID

My conclusion was that SOLID principles made this project easier to maintain and evolve than the others I saw, which looked more efficient because they had fewer files or methods.

In the client application, I used the Command pattern so new commands can be added while respecting the Open/Closed Principle.

### Design Patterns

I used the State pattern, for example, to make sure a game in progress can be paused and resumed.

The Builder pattern makes building a piece more flexible at run time.

To build the tetrominoes I used the Strategy pattern, because each piece has a different shape in each rotation, decided at run time. Each tetromino declares its shapes as a table of positions, and the base class builds the blocks from it.

For data persistence I used the Repository pattern: an interface in the domain model and an implementation outside it. Here the implementation is almost a mock that saves a flat file on the local disk, but it could easily be extended to a cloud model, for example.

In the client application, each keyboard action is a Command registered in a factory, so new commands can be added without changing the forms.

## Useful links

- [Download](https://github.com/phduarte/Tetris/releases/latest)
- [Documentation](https://github.com/phduarte/Tetris/wiki)

## Bibliography (in Portuguese)

- https://www.historiadomundo.com.br/idade-contemporanea/origem-do-tetris.htm#:~:text=Considerado%20um%20verdadeiro%20cl%C3%A1ssico%20dos,matem%C3%A1tico%20norte%2Damericano%20Solomon%20Golomb.
- https://tecnoblog.net/meiobit/289280/tetris-30-anos-a-historia/
