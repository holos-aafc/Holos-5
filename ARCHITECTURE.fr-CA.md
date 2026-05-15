[![en](https://img.shields.io/badge/lang-en-green.svg)](./ARCHITECTURE.md)

<!--markdownlint-disable MD025-->
# Guide d'architecture de l'application Holos
<!--markdownlint-enable MD025-->

> **🇨🇦 Traduction française en cours de préparation**
>
> Cette documentation technique destinée aux personnes contribuant au développement de Holos
> n'a pas encore été traduite professionnellement. La [version anglaise](./ARCHITECTURE.md)
> demeure la version faisant autorité jusqu'à ce qu'une traduction approuvée par les
> Services de traduction d'AAC soit mise en ligne.

## Aperçu

Holos est une application de bureau .NET 9 / Avalonia bâtie selon les modèles modernes
(MVVM, injection de dépendances par Prism + DryIoc). Le guide anglais détaille le processus
d'amorçage de l'application, les responsabilités de chaque service, et la façon dont la
chaîne de calcul du carbone est assemblée à partir de pièces faiblement couplées.

## Sujets abordés dans la version anglaise

Les liens ci-dessous pointent vers les sections correspondantes du
[guide anglais](./ARCHITECTURE.md) :

- [Vue d'ensemble](./ARCHITECTURE.md#overview) — pile technologique, modèles architecturaux.
- [Cadre d'application](./ARCHITECTURE.md#application-framework) — Avalonia UI, .NET 9, C# 13.
- [Modèles architecturaux](./ARCHITECTURE.md#architectural-patterns) — MVVM, injection de dépendances, cadre Prism.
- [Conteneur d'injection de dépendances](./ARCHITECTURE.md#dependency-injection-container) — DryIoc + Prism.DryIoc.
- [Processus d'amorçage de l'application](./ARCHITECTURE.md#application-bootstrap-process) — déroulement du démarrage.
- [App.axaml.cs — le chargeur d'amorçage](./ARCHITECTURE.md#starting-point-appaxamlcs---the-application-bootloader) — point d'entrée principal.
- [Diagramme de séquence d'injection de dépendances](./ARCHITECTURE.md#di-bootstrap-sequence-detailed) — diagramme Mermaid détaillé du démarrage.
- [Documentation connexe](./ARCHITECTURE.md#related-documentation) — liens vers les autres guides.

## Comment contribuer à la traduction

Les chaînes de l'interface utilisateur sont déjà traduites en français canadien — voir
`H.Localization/Resources/Strings/AppStrings.fr.resx`. Pour contribuer à la traduction
française de cette documentation technique, veuillez communiquer avec l'équipe Holos
d'AAC.

## Voir aussi

- [Version anglaise complète](./ARCHITECTURE.md)
- [Guide d'intégration des développeurs (anglais)](./DEVELOPER_ONBOARDING_GUIDE.md)
- [Guide de style de codage (anglais)](./CODING_STYLE_GUIDE.md)
- [README (français)](./README.fr-CA.md)
