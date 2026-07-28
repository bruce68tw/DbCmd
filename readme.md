# DbCmd

一個用於企業系統資料庫自動更新的 .NET Core Console 工具。

DbCmd 主要用來**自動執行資料庫更新作業**，適合應用在多資料庫、多系統模組的同步更新情境。

## 功能特色

### ✅ 支援從多個目錄讀取 SQL 檔案

每個目錄可以對應一個資料庫或系統模組，例如：

Sql
├── HR (人事資料庫)
├── CRM (客服資料庫)
├── ERP (ERP資料庫)
└── Common (共用更新)


方便依系統功能分類管理 SQL Script。

---

### ✅ 自動執行資料庫更新

只要將 SQL 檔案放入指定目錄，DbCmd 會依設定自動判斷並執行。

可應用於：

- 資料表新增或修改
- Stored Procedure 更新
- 系統版本升級
- 初始資料建立

---

### ✅ 每個 SQL 可設定執行時間

每支 SQL 可以設定執行條件，例如：

- 立即執行
- 每隔 6 小時執行
- 指定時間執行

讓不同更新作業可以依需求安排執行時機。

---

### ✅ 搭配 Windows 排程每小時啟動

透過 Windows Task Scheduler 設定每小時啟動 DbCmd。

DbCmd 每次啟動時會：

1. 掃描 SQL 目錄
2. 判斷哪些 SQL 需要執行
3. 執行符合條件的 Script
4. 記錄執行結果

不需要為每支 SQL 建立獨立排程。

---

## 適用情境

DbCmd 特別適合：

- 🔹 多客戶資料庫版本同步
- 🔹 ERP 系統版本更新
- 🔹 自動化部署流程
- 🔹 減少人工執行 SQL 的錯誤
- 🔹 多環境資料庫維護

---

## 使用架構

Windows Task Scheduler
|
▼
DbCmd
|
▼
SQL Script Manager
|
▼
SQL Directory
|
▼
Database Update


---

## 技術架構

- .NET Core Console Application
- SQL Script Automation
- Windows Task Scheduler
- Database Version Management

---

## Source Code

GitHub Repository:

https://github.com/bruce68tw/DbCmd.git
