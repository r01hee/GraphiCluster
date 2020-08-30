# GraphiCluster

Cluster Creator Kitのゲーム制作機能のコンポーネントとそのメッセージをグラフで可視化するツールです。

![top](https://raw.githubusercontent.com/wiki/r01hee/GraphiCluster/images/top.gif)

## Installation via UPM

Cluster Creator Kit導入済みのUnityプロジェクトで `Packages/manifest.json` を開き `"dependencies"` に `"dev.r01.graphicluster": "https://github.com/r01hee/GraphiCluster.git"` を追記して下さい。

```json
{
  // ...
  "dependencies": {
    "dev.r01.graphicluster": "https://github.com/r01hee/GraphiCluster.git",
    // ...
  }
  // ...
}
```

## Usage

1. Unityのメニューから「Cluster > GraphiCluster」を選択してください。  

![menu](https://raw.githubusercontent.com/wiki/r01hee/GraphiCluster/images/menu.gif)

2. GraphiClusterウィンドウの下にある「Reload」ボタンを押すと現在開いてるシーンのGraphが表示されます。  

![usage](https://raw.githubusercontent.com/wiki/r01hee/GraphiCluster/images/usage.gif)

## Features

### GameObject選択  
GameObjectアイコンのボタンを押すと対応するGameObjectがHierarchy上で選択されます。  

![selectgameobject](https://raw.githubusercontent.com/wiki/r01hee/GraphiCluster/images/selectgameobject.gif)

## License

[MIT license](LICENSE)