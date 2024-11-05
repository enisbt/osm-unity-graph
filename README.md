# osm-unity-graph
![Sample Map](https://i.imgur.com/DcGZxCM.png)

[![Unity 2021.3+](https://img.shields.io/badge/unity-2021.3.45+-blue)](https://unity3d.com/get-unity/download)
[![License: MIT](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/enisbt/osm-unity-graph/blob/master/LICENSE)

Tool for importing `.osm` ([OpenStreetMap](https://www.openstreetmap.org/)) maps to Unity as a 2D graph.

## Features
- Import `.osm` maps and convert them to Unity-compatible 2D graphs.
- Visualize nodes and ways.

## Installation

Import the .unitypackage file in the [Releases](https://github.com/enisbt/osm-unity-graph/releases/) page to your project.

## Usage

1. **Download the Map**:
   - Download your map from [OpenStreetMap](https://www.openstreetmap.org/).

2. **Open the Tool**:
   - Open the OSM Graph Tool in your Unity Editor's `Tools` menu.
   
   ![Sample Input](https://i.imgur.com/qCDgEZC.png)

3. **Import the `.osm` File**:
   - Select the `.osm` file you've downloaded from OpenStreetMap.

4. **Node Prefab**:
   - Can be an empty object, or you can create a custom prefab (e.g., a sphere) for visualizing nodes.

5. **Set Scale**:
   - Lower scale values generate larger maps but may impact performance.

6. **Generate the Graph**:
   - Click the "Generate Nodes and Ways" button to generate the map.

## License

Distributed under the MIT License. See `LICENSE` for more information.
