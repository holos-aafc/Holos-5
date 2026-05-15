[![en](https://img.shields.io/badge/lang-en-green.svg)](./CODING_STYLE_GUIDE.md)

<!--markdownlint-disable MD025-->
# Guide de style de codage Holos .NET
<!--markdownlint-enable MD025-->

> **🇨🇦 Traduction française en cours de préparation**
>
> Cette documentation technique destinée aux personnes contribuant au développement de Holos
> n'a pas encore été traduite professionnellement. La [version anglaise](./CODING_STYLE_GUIDE.md)
> demeure la version faisant autorité jusqu'à ce qu'une traduction approuvée par les
> Services de traduction d'AAC soit mise en ligne.

## Aperçu

Ce document décrit les conventions de codage et les directives de style utilisées dans
l'ensemble du code source de Holos. Le respect de ces conventions assure la cohérence, la
maintenabilité et la lisibilité du code à long terme.

## Sujets abordés dans la version anglaise

Les liens ci-dessous pointent vers les sections correspondantes du
[guide anglais](./CODING_STYLE_GUIDE.md) :

- [Conventions de nommage](./CODING_STYLE_GUIDE.md#naming-conventions) — classes, interfaces, méthodes, propriétés, champs, variables, énumérations.
- [Organisation du code](./CODING_STYLE_GUIDE.md#code-organization) — structure des fichiers, utilisation des régions.
- [Standards de documentation](./CODING_STYLE_GUIDE.md#documentation-standards) — documentation XML, commentaires.
- [Conventions du langage](./CODING_STYLE_GUIDE.md#language-conventions) — déclarations de variables, structure des méthodes, propriétés.
- [Gestion des erreurs](./CODING_STYLE_GUIDE.md#error-handling) — exceptions, validation, journalisation (logging).
- [Conventions de test](./CODING_STYLE_GUIDE.md#testing-conventions) — structure des classes de test, nommage des tests.
- [Directives propres au cadre](./CODING_STYLE_GUIDE.md#framework-specific-guidelines) — patrons MVVM Prism, pièges de liaison Avalonia XAML.

## Points particulièrement importants

Deux pièges sont les plus susceptibles d'affecter une nouvelle personne contribuant au
projet :

- **Piège de liaison Avalonia** : combiner `StringFormat` avec une liaison bidirectionnelle
  ou modifiable lance une `NotSupportedException` au moment de l'exécution. Utiliser
  `NumericUpDown.FormatString` ou des contextes en lecture seule. Voir la section
  [Avalonia XAML Binding Pitfalls](./CODING_STYLE_GUIDE.md#avalonia-xaml-binding-pitfalls).
- **Journalisation** : utiliser `_logger.LogX(...)` (via injection de dépendances) ou
  `_log.X(...)` (champ statique `NLog.Logger`) — ne **jamais** utiliser
  `System.Diagnostics.Trace.*`. La base de code utilise NLog uniquement, configuré par
  `H.GUI.Avalonia/H.Avalonia/NLog.config`.

## Comment contribuer à la traduction

Les chaînes de l'interface utilisateur sont déjà traduites en français canadien — voir
`H.Localization/Resources/Strings/AppStrings.fr.resx`. Pour contribuer à la traduction
française de cette documentation technique, veuillez communiquer avec l'équipe Holos
d'AAC.

## Voir aussi

- [Version anglaise complète](./CODING_STYLE_GUIDE.md)
- [Guide d'architecture (anglais)](./ARCHITECTURE.md)
- [Guide d'intégration des développeurs (anglais)](./DEVELOPER_ONBOARDING_GUIDE.md)
- [README (français)](./README.fr-CA.md)
