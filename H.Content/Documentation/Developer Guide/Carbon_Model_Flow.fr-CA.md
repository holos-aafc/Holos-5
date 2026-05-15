[![en](https://img.shields.io/badge/lang-en-green.svg)](./Carbon_Model_Flow.md)

<!--markdownlint-disable MD025-->
# Flux du modèle de carbone — Vue → Analyse → Résultats
<!--markdownlint-enable MD025-->

> **🇨🇦 Traduction française en cours de préparation**
>
> Cette documentation technique destinée aux personnes contribuant au pipeline de calcul
> du carbone n'a pas encore été traduite professionnellement. La
> [version anglaise](./Carbon_Model_Flow.md) demeure la version faisant autorité jusqu'à
> ce qu'une traduction approuvée par les Services de traduction d'AAC soit mise en ligne.

## Aperçu

Diagramme de flux de bout en bout du pipeline d'analyse des gaz à effet de serre (GES) et
du carbone dans Holos v5. Le document anglais comprend un diagramme Mermaid principal qui
suit le cheminement d'un champ de blé simple depuis la saisie par l'utilisateur dans
l'interface graphique jusqu'à l'affichage des résultats GES, ainsi qu'un sous-diagramme
montrant la répartition du pipeline animal entre six services par espèce (bovins de
boucherie, laitiers, porcs, ovins, volaille, autres animaux).

Le document est particulièrement utile pour :

- l'intégration d'une nouvelle personne contribuant au code du carbone ;
- le débogage d'un graphique vide ou de valeurs `NaN` dans les résultats du carbone du sol ;
- décider où ajouter une nouvelle étape de calcul — le diagramme montre ce qui s'exécute
  avant et après `AssignCarbonInputs` et `CalculateFinalResultsForField`.

## Sujets abordés dans la version anglaise

Les liens ci-dessous pointent vers les sections correspondantes du
[document anglais](./Carbon_Model_Flow.md) :

- [Flux principal](./Carbon_Model_Flow.md#flow) — diagramme Mermaid de bout en bout (vue → analyse → résultats).
- [Points non évidents](./Carbon_Model_Flow.md#things-the-diagram-doesnt-make-obvious) — pièges et invariants d'ordre que le diagramme ne montre pas.
- [Répartition du pipeline animal](./Carbon_Model_Flow.md#animal-pipeline-dispatch) — diagrammes d'héritage et de répartition pour les six services par espèce.
  - [Héritage](./Carbon_Model_Flow.md#inheritance) — diagramme de classes.
  - [Répartition](./Carbon_Model_Flow.md#dispatch) — organigramme de répartition.
  - [Tables de coefficients par service](./Carbon_Model_Flow.md#coefficient-tables-consulted-by-each-service) — tableau récapitulatif.
  - [Cheminement d'un composant animal](./Carbon_Model_Flow.md#how-a-component-flows-through) — explication en cinq étapes.
  - [Pourquoi les bovins et les laitiers partagent une classe de base supplémentaire](./Carbon_Model_Flow.md#why-beef-and-dairy-share-an-extra-base) — justification.
- [Où les modes de défaillance se manifestent](./Carbon_Model_Flow.md#where-the-failure-modes-surface) — tableau de diagnostic.
- [Index des fichiers et classes](./Carbon_Model_Flow.md#file--class-index) — emplacement de chaque étape dans le code source.

## Comment contribuer à la traduction

Les chaînes de l'interface utilisateur sont déjà traduites en français canadien — voir
`H.Localization/Resources/Strings/AppStrings.fr.resx`. Pour contribuer à la traduction
française de cette documentation technique, veuillez communiquer avec l'équipe Holos
d'AAC.

## Voir aussi

- [Version anglaise complète](./Carbon_Model_Flow.md)
- [Guide du développeur (anglais)](./Developer_Guide_EN.md)
- [Guide d'architecture (anglais)](../../../ARCHITECTURE.md)
- [README (français)](../../../README.fr-CA.md)
