[![en](https://img.shields.io/badge/lang-en-green.svg)](./DEVELOPER_ONBOARDING_GUIDE.md)

<!--markdownlint-disable MD025-->
# Guide d'intégration des développeurs Holos
<!--markdownlint-enable MD025-->

> **🇨🇦 Traduction française en cours de préparation**
>
> Cette documentation technique destinée aux personnes qui commencent à contribuer à Holos
> n'a pas encore été traduite professionnellement. La
> [version anglaise](./DEVELOPER_ONBOARDING_GUIDE.md) demeure la version faisant autorité
> jusqu'à ce qu'une traduction approuvée par les Services de traduction d'AAC soit mise
> en ligne.

## Aperçu

Ce guide aide les nouvelles personnes contribuant au projet à configurer leur environnement
de développement pour Holos. Holos est une application de bureau sophistiquée de calcul de
l'empreinte carbone et de gestion des fermes, bâtie avec .NET 9, l'interface utilisateur
Avalonia, et les modèles architecturaux modernes.

## Sujets abordés dans la version anglaise

Les liens ci-dessous pointent vers les sections correspondantes du
[guide anglais](./DEVELOPER_ONBOARDING_GUIDE.md) :

- [Prérequis](./DEVELOPER_ONBOARDING_GUIDE.md#prerequisites) — exigences matérielles, comptes nécessaires.
- [Installation des outils de développement](./DEVELOPER_ONBOARDING_GUIDE.md#development-tools-installation) — SDK .NET 9, Visual Studio 2022, Visual Studio Code, Git.
- [Configuration du dépôt](./DEVELOPER_ONBOARDING_GUIDE.md#repository-setup) — clonage du dépôt, structure du projet.
- [Configuration du projet](./DEVELOPER_ONBOARDING_GUIDE.md#project-configuration) — restauration des paquets NuGet, vérification des références.
- [Compilation de la solution](./DEVELOPER_ONBOARDING_GUIDE.md#building-the-solution) — Visual Studio, Visual Studio Code, ligne de commande.
- [Exécution de l'application](./DEVELOPER_ONBOARDING_GUIDE.md#running-the-application) — démarrage de l'interface graphique ou de l'interface en ligne de commande.
- [Flux de travail de développement](./DEVELOPER_ONBOARDING_GUIDE.md#development-workflow) — stratégie de branches, normes de codage, tests, débogage.
- [Dépannage](./DEVELOPER_ONBOARDING_GUIDE.md#troubleshooting) — erreurs de compilation, problèmes d'exécution, problèmes Git.
- [Outils facultatifs](./DEVELOPER_ONBOARDING_GUIDE.md#optional-tools) — outils de productivité tiers (non endossés officiellement par le projet).

## Points particulièrement importants

- **Cadre cible** : la solution cible `.net9.0` (et non `.net9.0-windows`). Vous avez besoin
  du **SDK** .NET 9, pas seulement de l'exécution (« runtime »).
- **Trois IDE pris en charge** : Visual Studio 2022, Visual Studio Code, ou JetBrains
  Rider. Le projet n'endosse officiellement aucun IDE en particulier ; les trois sont
  également pris en charge.
- **Fichier de solution** : `Holos.sln` à la racine du dépôt.

## Comment contribuer à la traduction

Les chaînes de l'interface utilisateur sont déjà traduites en français canadien — voir
`H.Localization/Resources/Strings/AppStrings.fr.resx`. Pour contribuer à la traduction
française de cette documentation technique, veuillez communiquer avec l'équipe Holos
d'AAC.

## Voir aussi

- [Version anglaise complète](./DEVELOPER_ONBOARDING_GUIDE.md)
- [Guide d'architecture (anglais)](./ARCHITECTURE.md)
- [Guide de style de codage (anglais)](./CODING_STYLE_GUIDE.md)
- [README (français)](./README.fr-CA.md)
- [Code de conduite (français)](./CODE_OF_CONDUCT.fr-CA.md)
- [Lignes directrices pour les contributeurs (français)](./CONTRIBUTING.fr-CA.md)
