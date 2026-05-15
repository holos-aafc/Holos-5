[![en](https://img.shields.io/badge/lang-en-green.svg)](./Developer_Guide_EN.md)

<!--markdownlint-disable MD025-->
# Guide du développeur Holos
<!--markdownlint-enable MD025-->

> **🇨🇦 Traduction française en cours de préparation**
>
> Cette documentation technique destinée aux personnes contribuant à Holos n'a pas encore
> été traduite professionnellement. La [version anglaise](./Developer_Guide_EN.md) demeure
> la version faisant autorité jusqu'à ce qu'une traduction approuvée par les Services de
> traduction d'AAC soit mise en ligne.

## Aperçu

Référence de démarrage pour les nouvelles personnes contribuant à Holos. Le guide anglais
couvre la configuration de chacun des trois IDE pris en charge (Visual Studio 2022, Visual
Studio Code, JetBrains Rider), les commandes de la CLI `dotnet`, la disposition de la
solution, le modèle de journalisation NLog, le flux de travail de localisation, et le
diagramme de navigation de l'interface graphique.

## Sujets abordés dans la version anglaise

Les liens ci-dessous pointent vers les sections correspondantes du
[guide anglais](./Developer_Guide_EN.md) :

- [Localisation](./Developer_Guide_EN.md#localization) — ajout d'une nouvelle chaîne visible par l'utilisateur en anglais et en français.
- [Prérequis](./Developer_Guide_EN.md#prerequisites) — SDK .NET 9, Git, Windows 10/11.
- [Option 1 : Visual Studio 2022](./Developer_Guide_EN.md#option-1-visual-studio-2022) — installation, extension Avalonia, projet de démarrage.
- [Option 2 : Visual Studio Code](./Developer_Guide_EN.md#option-2-visual-studio-code) — extensions, commandes CLI, configuration du débogage, conseils.
- [Option 3 : JetBrains Rider](./Developer_Guide_EN.md#option-3-jetbrains-rider) — configuration de base.
- [Disposition de la solution](./Developer_Guide_EN.md#solution-layout) — projets de premier niveau et leur rôle.
- [Carte de navigation de l'interface graphique](./Developer_Guide_EN.md#gui-navigation-map) — diagramme Mermaid du déroulement des écrans.
- [Journalisation](./Developer_Guide_EN.md#logging) — modèle NLog, destinations des journaux.
- [Tests](./Developer_Guide_EN.md#tests) — projets MSTest, lignes de base de tests.
- [Style de codage](./Developer_Guide_EN.md#coding-style) — pointeur vers le guide de style + pièges courants.
- [Travailler avec le modèle de carbone](./Developer_Guide_EN.md#working-with-the-carbon-model) — pointeur vers le diagramme de flux du modèle de carbone.

## Comment contribuer à la traduction

Les chaînes de l'interface utilisateur sont déjà traduites en français canadien — voir
`H.Localization/Resources/Strings/AppStrings.fr.resx`. Pour contribuer à la traduction
française de cette documentation technique, veuillez communiquer avec l'équipe Holos
d'AAC.

## Voir aussi

- [Version anglaise complète](./Developer_Guide_EN.md)
- [Diagramme de flux du modèle de carbone (anglais)](./Carbon_Model_Flow.md)
- [Guide d'architecture (anglais)](../../../ARCHITECTURE.md)
- [Guide d'intégration des développeurs (anglais)](../../../DEVELOPER_ONBOARDING_GUIDE.md)
- [README (français)](../../../README.fr-CA.md)
