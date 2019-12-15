# RobotApi

Robot API を使ったサンプルプログラムを収録します。

Robot API を利用するには、ローカルコンピューターで稼働中のロボットサービスが Orchestrator に接続し、
ログオンユーザーに対応するローカルコンピューターで稼働可能なロボットが同 Orchestrator に定義されている必要があります。

.NETプロジェクトにおいては、UiPath Studio のインストールフォルダーにある UiPath.Robot.Api.dll の参照を追加してください。

## RunInParallel

コマンド行引数で与えられたプロセス名のプロセスを並列実行するコンソールアプリケーションです。

なお、フォアグラウンドプロセスは1個まで実行できます。2個目の実行を試みるとエラーとなります。

バックグランドプロセスは1個以上実行できます。

## UiPathTeam.EasyProcessing.Activities

UiPath.System.Activities パッケージに既にありますが、プロセスを呼び出しアクティビティを Robot API を利用して実装してみました。

フローチャートで使った場合にプロセス名を表示したり、プロセス名の一覧から選択可能なところが、System パッケージのオリジナルと違うところです。
