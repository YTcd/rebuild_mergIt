- DragEnd 시점에서 같은 아이콘이라면 파괴하는 로직 적용
- 아직 GridHandler에서 Release하는 로직을 쓰질 않아서 엉뚱한 곳에서 터지는 중
- 5보다 인덱스 올라가면 안 합쳐지는 조건 추가해야할듯
- 코루틴+Lerp 방식 일부 수정 필요


- camera orthographicSize
- array 2차원 배열과 Jagged 배열