# Postal Codes JP Web API
[![license](https://img.shields.io/github/license/kcg-edu-future-lab/Postal-Codes-JP.svg)](https://github.com/kcg-edu-future-lab/Postal-Codes-JP/blob/master/LICENSE)
[![GitHub release](https://img.shields.io/github/release/kcg-edu-future-lab/Postal-Codes-JP.svg)](https://github.com/kcg-edu-future-lab/Postal-Codes-JP/releases)

日本の郵便番号を検索するための Web API を提供します。  
Provides the Web API to search postal codes of Japan.

この Web API のデータ形式は JSON で、CORS (Cross-Origin Resource Sharing) をサポートしています。

## Web API のご利用について
Postal Codes JP Web API のテスト用サイトとして提供されている [postal-codes-jp.azurewebsites.net](https://postal-codes-jp.azurewebsites.net/) は、商用・非商用を問わず本番運用ではお使いいただけません。
この Web API の事前調査の目的でお使いいただけます。  
また、事情によりこのサイトの運用を休止する場合があります。

このプロジェクトでは、**各利用者 (アプリ開発者) が Web API をホストして運用することを想定しています**。  
詳細は [ホスティングについて](docs/Hosting.md) をご参照ください。

## Specification
[テスト用サイト](https://postal-codes-jp.azurewebsites.net/)のトップページがヘルプとなっており、API の仕様を確認できます。  
ヘルプページとして、OpenAPI (Swagger) を利用しています。  
テスト用の UI も付属しており、このページ上で API を呼び出すことができます。

[![](docs/images/Swagger-Top-v1.png)](https://postal-codes-jp.azurewebsites.net/)

### APIs
| 分類 | 説明 |
-|-
| Prefs | 都道府県のデータを取得します。 |
| Cities | 市区町村のデータを取得します。 |
| Towns | 町域名から、郵便番号と町域のデータを取得します。 |
| PostalCodes | 郵便番号から、郵便番号と町域のデータを取得します。 |
| Search | 任意のキーワードから、郵便番号と町域のデータを取得します。 |

### PostalCodes
基本的な機能として、郵便番号から町域を検索できます。  
郵便番号には 3～7 桁を指定できます。前方一致検索です。
```
https://domain/api/postalcodes/6011387
https://domain/api/postalcodes/601-138
https://domain/api/postalcodes/601
```

応答の JSON
```json
[
  {"postalCode":"6011387","name":"醍醐一ノ切町","kana":"だいごいちのきりちょう","remarks":"","city":{"code":"26109","name":"京都市伏見区","kana":"きょうとしふしみく","pref":{"code":"26","name":"京都府","kana":"きょうとふ"}}},
  {"postalCode":"6011387","name":"醍醐三ノ切","kana":"だいごさんのきり","remarks":"","city":{"code":"26109","name":"京都市伏見区","kana":"きょうとしふしみく","pref":{"code":"26","name":"京都府","kana":"きょうとふ"}}},
  {"postalCode":"6011387","name":"醍醐二ノ切町","kana":"だいごにのきりちょう","remarks":"","city":{"code":"26109","name":"京都市伏見区","kana":"きょうとしふしみく","pref":{"code":"26","name":"京都府","kana":"きょうとふ"}}}
]
```

### Search
任意のキーワードから町域を検索できます。  
空白文字で区切って複数のキーワードを指定できます。部分一致検索です。
```
"京都府 ヒルズ"
https://domain/api/search?q=%E4%BA%AC%E9%83%BD%E5%BA%9C+%E3%83%92%E3%83%AB%E3%82%BA
"烏　条"
https://domain/api/search?q=%E7%83%8F%E3%80%80%E6%9D%A1
```

応答の JSON
```json
[
  {"postalCode":"6200067","name":"荒河ヒルズ","kana":"あらがひるず","remarks":"","city":{"code":"26201","name":"福知山市","kana":"ふくちやまし","pref":{"code":"26","name":"京都府","kana":"きょうとふ"}}}
]
```

その他の詳細は [Wiki](https://github.com/kcg-edu-future-lab/Postal-Codes-JP/wiki) をご参照ください。

### Data
日本郵便で提供されている [郵便番号データ](http://www.post.japanpost.jp/zipcode/download.html) を加工して利用しています。

```csv
26102,"602  ","6028364","ｷｮｳﾄﾌ","ｷｮｳﾄｼｶﾐｷﾞｮｳｸ","ﾂｷﾇｹﾁｮｳ(ｼﾓﾀﾞﾁｳﾘﾄﾞｵﾘｵﾝﾏｴﾆｼｲﾙ､ｼﾓﾀﾞﾁｳﾘﾄﾞｵﾘｵﾝﾏｴﾆｼｲﾙｻｶﾞﾙ､ｼﾓﾀﾞﾁｳﾘﾄﾞｵﾘﾃﾝ","京都府","京都市上京区","突抜町（下立売通御前西入、下立売通御前西入下る、下立売通天",0,0,0,0,0,0
26102,"602  ","6028364","ｷｮｳﾄﾌ","ｷｮｳﾄｼｶﾐｷﾞｮｳｸ","ｼﾞﾝﾐﾁﾋｶﾞｼｲﾙ､ｼﾓﾉｼﾓﾀﾞﾁｳﾘﾄﾞｵﾘｵﾝﾏｴﾆｼｲﾙ)","京都府","京都市上京区","神道東入、下の下立売通御前西入）",0,0,0,0,0,0
```

例えば、この CSV のデータは次のように加工されます。
- 1 つのデータが複数行に分割されているため、1 行に加工する
- 読み仮名 (半角カタカナ) をひらがなに変更する
- `（）` 内のデータを備考 (Remarks) に移動する

```json
[
  {"postalCode":"6028364","name":"突抜町","kana":"つきぬけちょう","remarks":"下立売通御前西入、下立売通御前西入下る、下立売通天神道東入、下の下立売通御前西入","city":{"code":"26102","name":"京都市上京区","kana":"きょうとしかみぎょうく","pref":{"code":"26","name":"京都府","kana":"きょうとふ"}}}
]
```

詳細は [データの加工](https://github.com/kcg-edu-future-lab/Postal-Codes-JP/wiki/2.3.%E3%83%87%E3%83%BC%E3%82%BF%E3%81%AE%E5%8A%A0%E5%B7%A5) をご参照ください。

## Release Notes
- **v1.0.18** 初版リリース。インターフェイスを改良。
- **v1.0.7** β版リリース。

### Future Plans
- 各配置サーバー自身で月次データをダウンロードおよび加工。
- ローマ字のデータを追加。
- 事業所のデータを追加。

## Development Environment
### Web API
- .NET Core 2.0
- ASP.NET Core 2.0.8
- Microsoft.AspNetCore.Cors 2.0.3
- Swashbuckle.AspNetCore 2.5.0

### Data Console
- .NET Framework 4.7
- [Bellona.Analysis](https://github.com/sakapon/Bellona.Analysis)
- (EntityFramework)
- (EntityFramework.SqlServerCompact)

### Data
- [日本郵便 郵便番号データ](http://www.post.japanpost.jp/zipcode/download.html)

### Records
2018.06.21-29 京都コンピュータ学院 未来環境ラボ 「Re:京都オープンデータハッカソン」
