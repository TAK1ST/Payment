\##\[FLOW ACID](https://mermaid.live/edit#pako:eNqFVMFu00AQ\_ZXRnlIpLY6TuLUFQUl8qQC1ouWCfJnYm2RVe9fsroE06gdwgQtHJH6D7-EH4BOYtZ0mShrhiz2eN\_Nm3szumqUq4yxihn-ouEx5LHChsUgk0FOitiIVJUoL7wzXgKZ-H3qvcVVwacdl6TCtBWQeQuOJg8RocYaGH\_onKO8con6Pry8T2WAc8elotGWKYJqL9A4SRv9g6Hl3CWugUlkOWiyWFtQcdiNuLDHBrUZpMLVCyU32LYg44gnlXnLKPcMcSRXojF7UDC9PGng8Od2rZdIiP2IuMvj94\_ufX9-gM1XSCGNJ2dXJUaqYZ1Vq6\_ww16potB4\_1YuDX5Vco6sdSi4zIRfQGVtVkH72CImTMoK3bsTGgnXdz4mhJrQKbnieb4bqoPut3W4CTJWm3JijjYyzbC8pTI51MVVFmXP6O1N2CVWZoeXmv520kYXYG-LTHNdcO\_WBcqOrKVeL5zP9bASduNI4E\_kuz-FIW6KrVweFENDNKHrc9FYZ-Pvz65dNQlpq8mskHfLdak3jPrLTY2pjScJVbgdKXBkwWHAQlhfbuANNXitaVocBrT5B59KovF6Rdl1pTxLJumyhRcYiqyveZQXXBTqTrR0oYURb8IRF9JnxOVa5defpgcLoXL5XqthEalUtliyaY27IakbX3huPEGLkeqoqaVnUO\_frHCxas89kDs7PfD8Iw34Q9MJ-n5wrFvn-mRcGveHwPAyG\_Z43vHjosvua1Tu7uAjDQTDw\_IE\_GAZe0GU8E1bpN83lVd9hD\_8AJ-GTLg)



\## 1. \*\*Atomicity (Tính nguyên tử)\*\*



\* \*\*Khái niệm\*\*: Một transaction hoặc thực hiện \*toàn bộ\*, hoặc \*không gì cả\*.

\* \*\*Trong Payment\*\*: Khi bạn bấm “Thanh toán 500k để mua iPhone ảo trên Shopee”, thì:



  1. Tài khoản A (người mua) trừ 500k.

  2. Tài khoản B (người bán) cộng 500k.

     Nếu một trong hai bước fail (ví dụ: trừ tiền thành công nhưng cộng cho người bán fail), thì toàn bộ transaction bị rollback → số dư trở về trạng thái ban đầu.

\* \*\*Ý nghĩa\*\*: Tránh tình huống “tiền bốc hơi” hoặc “tạo ra tiền ảo”.



---



\## 2. \*\*Consistency (Tính nhất quán)\*\*



\* \*\*Khái niệm\*\*: Sau mỗi transaction, hệ thống phải ở trạng thái hợp lệ, tuân thủ các ràng buộc.

\* \*\*Trong Payment\*\*:



  \* Số dư không thể âm (trừ khi cho phép overdraft).

  \* Tổng số tiền trong hệ thống (A + B) phải giữ nguyên sau giao dịch.

  \* Các ràng buộc như “mỗi đơn hàng chỉ thanh toán 1 lần” cũng phải được đảm bảo.

\* \*\*Ví dụ\*\*: Nếu user có 300k mà cố gắng mua món 500k → transaction phải fail ngay từ đầu, không để database rơi vào trạng thái nợ bậy bạ.



---



\## 3. \*\*Isolation (Tính cô lập)\*\*



\* \*\*Khái niệm\*\*: Transaction đang chạy không nên bị transaction khác làm rối loạn.

\* \*\*Trong Payment\*\*:



  \* Hai user cùng lúc bấm thanh toán 500k để mua \*cùng một sản phẩm cuối cùng\* (chỉ còn 1 cái).

  \* Nếu isolation yếu → cả hai đều được “xác nhận thanh toán”, tạo ra tình trạng bán một sản phẩm cho nhiều người.

  \* Với isolation chuẩn → hệ thống sẽ lock product hoặc balance, xử lý từng transaction, người đến sau sẽ fail.

\* \*\*Ý nghĩa\*\*: Tránh race condition (trạng thái chạy đua) làm hỏng dữ liệu.

\* \*\*Isolation Levels\*\* (ví dụ):



  \* \*\*Read Uncommitted\*\*: Dễ bị dirty read (nghe ngóng kết quả tạm thời).

  \* \*\*Read Committed\*\*: An toàn hơn, chỉ đọc dữ liệu đã commit.

  \* \*\*Repeatable Read\*\*: Giữ ổn định khi đọc nhiều lần.

  \* \*\*Serializable\*\*: Cứng rắn nhất, nhưng chậm → đảm bảo như xếp hàng từng đứa.



---



\## 4. \*\*Durability (Tính bền vững)\*\*



\* \*\*Khái niệm\*\*: Một khi transaction commit thành công thì phải tồn tại vĩnh viễn, kể cả khi hệ thống crash.

\* \*\*Trong Payment\*\*:



  \* Người mua chuyển tiền thành công, hệ thống báo “Thanh toán hoàn tất”.

  \* Sau đó server cháy, DB restart → thông tin transaction vẫn còn (nhờ log, replication, hoặc write-ahead log).

\* \*\*Ý nghĩa\*\*: Không có chuyện “tiền đã mất nhưng database quên ghi lại”.



---



\## 5. \*\*Khi áp dụng vào Payment System\*\*



\* \*\*Atomicity\*\* → không có vụ tiền trừ rồi biến mất.

\* \*\*Consistency\*\* → không có số dư âm hoặc đơn hàng thanh toán nhiều lần.

\* \*\*Isolation\*\* → tránh tranh cướp tài nguyên (double spending, overselling).

\* \*\*Durability\*\* → hệ thống chết máy cũng không mất transaction đã xác nhận.



---



\## 6. \*\*Mở rộng: Payment thực tế thường còn thêm\*\*



\* \*\*Idempotency\*\*: Lệnh thanh toán gửi lại nhiều lần chỉ thực hiện \*một lần duy nhất\*. (Khách bấm F5 không thể trả tiền 2 lần).

\* \*\*Distributed Transaction\*\*: Nhiều service cùng tham gia (Payment Service, Order Service, Bank API). Thường khó giữ full ACID → hay dùng \*\*saga pattern\*\* hoặc \*\*eventual consistency\*\*.

\* \*\*Logging/Audit Trail\*\*: Lưu lại tất cả lịch sử để truy vết nếu có tranh chấp.

