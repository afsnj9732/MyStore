專案簡介:

    結合 Vue.js、.NET Core Web API 和 Microsoft SQL Server 撰寫的前後端分離全端商城網站


    後端   使用非同步語法與三層式架構撰寫的Web API


    資料庫 使用關聯式資料表設計，並使用Entity Framework 進行資料存取，減少遭受隱碼攻擊的風險


    前端   使用Vue語法，自動進行Html編碼，並避免直接嵌入後端回傳值，以減少XSS攻擊的風險


    會員   註冊、登入、登出，串接reCAPTCHA自動進行機器人辨識

           身分驗證使用JWT，以降低CSRF攻擊的風險

           密碼使用BCrypt加密，以避免明文儲存在資料庫


    商品    搜尋、分頁，使用資料庫索引加快查詢速度


    購物車  新增、刪除、修改，使用預存程序縮減運行所需時間


    訂單    查詢、結帳，串接Stripe 第三方金流，並使用 Transaction 以保障資料庫變動的一致性



