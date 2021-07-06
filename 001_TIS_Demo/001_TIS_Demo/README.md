WebDevelopmentTemplate
===============

Overview

## Description

## 実装方法

### Entity Framework Code First Migration
下記記載のソースコード部分は、パッケージマネージャコンソールで実行する
#### 開発準備
1. データベースを作成して初めの1回だけ行う

        enable-migrations

2. DbContextクラス作成

#### スキーマ変更
1. テーブル追加

    1. テーブルクラスを作成する

    2. DbContextに、作成したテーブルクラスを登録する
    
2. テーブル変更
    1. テーブルクラスを変更する

3. migration用コード生成  
前回以降の上記の変更を行うコードを生成する

        add-migration ファイル名

4. DBへの反映

        update-database
        
5. 特定バージョンへの変更

        update-database –TargetMigration: コード生成でつけた名前


## デプロイ
### DB
#### 新規

アプリケーションを実行する

#### 更新

        Update-Database -Script -SourceMigration: $InitialDatabase

で作成されたSQLを実行する

## Requirement

## Usage

## Install

## Contribution
