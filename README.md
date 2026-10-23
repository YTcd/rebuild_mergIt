# rebuild_mergit

9x9 보드에서 같은 레벨의 아이템을 합쳐 점수를 올리는 Unity 2D 머지 퍼즐 게임

- 버튼으로 빈 칸에 아이템 생성 (점수에 따라 생성 레벨 상승)
- 드래그 또는 Auto Merge 버튼으로 같은 레벨 아이템 합성
- 최대 레벨 아이템을 더블클릭하면 점수로 전환
- 해상도/화면 비율에 맞춰 보드와 UI 자동 배치

Unity 6000.2.12f1

### [Play on Web](https://ytcd.github.io/rebuild_mergIt/)

---

## 공부한 내용

- camera orthographicSize
- array 2차원 배열과 Jagged 배열
- sprite의 pixel size와 unit size(world size)의 차이가 발생하는 이유와 보정
- UnityEngine.Pool.ObjectPool<T>
- [TryGetComponent](https://velog.io/@lovemytae/Try%ED%8C%A8%ED%84%B4)
- [Unity lazy singleton](https://velog.io/@lovemytae/Unity-Lazy-Singletone)
- destroyCancellationToken
- Animator, Animator Controller, AnimationClip
- Input System
