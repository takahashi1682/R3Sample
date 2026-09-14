# 10_2. Concat

## この回で学ぶこと
- `Observable.Concat`で複数のストリームを「順番につなげる」方法を理解する
- Concatでは後ろのストリームが、前のストリームの完了後にしか購読されないことを理解する
- MergeとConcatの違い（同時に流れるか、順番待ちするか）を説明できるようになる

## 前提知識
- Observable / Subject / Subscribe の基本（0〜4の回）
- `OnNext` / `OnCompleted`を手動で呼び出してストリームの流れを確認する方法
- `AddTo(this)`によるリソース管理
- 10_1_Mergeの内容（Mergeとの比較で理解が深まります）

## 概念解説
Concatは、複数のストリームを「順番につなげる」オペレーターです。

イメージとしては、リレー走です。1走者（最初のストリーム）が走り終わる（`OnCompleted`する）まで、2走者（次のストリーム）はバトンを受け取れず、スタートラインで待機しています。1走者がゴールして初めて、2走者が走り始めます。

```
subject1: --A--B--|              (| = Completed)
subject2:          -----C--D--|
Concat  : --A--B----------C--D--|
```

- 最初のストリームが値を流している間、後ろのストリームに値を流しても**無視されます**（まだ購読されていないため）
- 最初のストリームが`OnCompleted`した時点で、初めて次のストリームの購読が始まる
- 最後のストリームが`OnCompleted`した時点で、Concat後のストリームも`OnCompleted`になる

Mergeとの違いは明確です。Mergeは全てのストリームを最初から同時に購読し、来た順に値を流しますが、Concatは「順番」を保証するために、前のストリームが終わるまで次のストリームを待たせます。「同時に複数のイベントを受け取りたい」ならMerge、「Aが終わってからBをやりたい」といった順序性が重要な場面ではConcatを使う、と覚えるとよいでしょう。

## シーンの操作方法
1. `10_2_Concat`シーンを開いてPlayします。
2. Consoleに`next: Subject1`が1回だけ出力され、その時点では`_subject2`に流した値は無視されていることを確認してください。
3. `_subject1.OnCompleted()`が呼ばれた後で`next: Subject2`が出力されることを確認してください。
4. 最後に`_subject2.OnCompleted()`が呼ばれて`completed.`が出力されることを確認してください。

## コード解説
```csharp
Observable
    .Concat(
        _subject1, // 順序1
        _subject2 // 順序2(1が完了した後に流れる)
    ).Subscribe(...)
```
`Observable.Concat`には合成したいストリームを渡す順番に意味があります。ここでは`_subject1`が先に購読され、`_subject1`が完了した後に`_subject2`が購読されます。

```csharp
_subject1.OnNext("Subject1");
_subject2.OnNext("Subject2"); // 流れない
_subject1.OnCompleted(); // この時点でsubject2が購読される
```
`_subject2`はまだ購読されていないため、この時点で`OnNext`を呼んでも値は捨てられ、ログには出力されません。`_subject1.OnCompleted()`が呼ばれて初めて、内部的に`_subject2`の購読が始まります。

```csharp
_subject1.OnNext("Subject1"); // 流れない
_subject2.OnNext("Subject2");
_subject2.OnCompleted();
```
すでに`_subject1`は完了済みのため、以降`_subject1`に値を流しても無視されます。`_subject2`はすでに購読済みなので、ここでの`OnNext`はそのままログに出力されます。最後に`_subject2.OnCompleted()`が呼ばれることで、Concat全体も完了します。

## 実行結果の例
```
next: Subject1
next: Subject2
completed.
```
（`_subject2.OnNext("Subject2")`の1回目の呼び出しと、`_subject1.OnNext("Subject1")`の2回目の呼び出しは、いずれも無視されてログに出力されません）

## よくあるつまずきポイント
- Mergeと同じ感覚で「両方に流した値がすぐ出力される」と思い込みがちですが、Concatでは**後ろのストリームは前が完了するまで一切購読されていません**。値を流しても静かに捨てられるだけで、エラーにもなりません。
- 「後ろのストリームに値を流すタイミングを間違えても後で流れてくる」と誤解しないようにしましょう。購読される前に流した値は失われ、二度と出てきません。
- Concatを使う場合、前のストリームが`OnCompleted()`を呼ばない（＝いつまでも完了しない）と、後ろのストリームには永久に切り替わりません。

## 理解度チェック
1. `_subject1`が一度も`OnCompleted()`を呼ばなかった場合、`_subject2`に値を流すとログはどうなりますか。
2. コード中の1回目の`_subject2.OnNext("Subject2")`がログに出力されない理由を説明してください。
3. 「AボタンとBボタンのどちらが押されてもよいので、押された値を受け取りたい」という要件にはMergeとConcatのどちらが適していますか。理由も添えて答えてください。
