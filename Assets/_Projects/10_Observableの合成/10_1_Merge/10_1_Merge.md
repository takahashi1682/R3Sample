# 10_1. Merge

## この回で学ぶこと
- `Observable.Merge`で複数のストリームを1本にまとめる方法を理解する
- Mergeが「値をそのまま合流させるだけ」のシンプルな合成であることを理解する
- Mergeの`OnCompleted`が発火するタイミング（全ての合成元が完了したとき）を理解する

## 前提知識
- Observable / Subject / Subscribe の基本（0〜4の回）
- `OnNext` / `OnCompleted`を手動で呼び出してストリームの流れを確認する方法
- `AddTo(this)`によるリソース管理

## 概念解説
Mergeは、この単元の中でいちばんシンプルな合成オペレーターです。

イメージとしては、2本の川（ストリーム）が合流して1本の川になるようなものです。合流した後の川には、元のどちらの川から流れてきた水（値）も、来た順番のまま流れ込みます。値を待たせたり、組み合わせたりすることは一切しません。

```
subject1: --A--------C--->
subject2: -----B----------D->
Merge   : --A--B-----C---D->
```

- 値は「流れてきた順」にそのまま出力される
- 複数のストリームのどれから来たかは区別されない（値の中身だけが流れる）
- 全ての合成元ストリームが`OnCompleted`になって初めて、Merge後のストリームも`OnCompleted`になる（1本でも完了していないストリームがあれば、Mergeはまだ完了しない）

他の合成オペレーターとの違いを一言でいうと、Mergeは「値を待ち合わせない」という点が特徴です。この後学ぶZipやCombineLatestは、複数のストリームの値を組み合わせるために「相手の値を待つ」動きをしますが、Mergeにはその概念がありません。来た値をそのまま流すだけです。

## シーンの操作方法
1. `10_1_Merge`シーンを開いてPlayします。
2. `Start`が実行された瞬間に、`_subject1`→`_subject2`の順でログが出力されます。
3. その後、`_subject1.OnCompleted()`と`_subject2.OnCompleted()`が呼ばれ、両方が完了した時点で`completed.`が出力されることを確認してください。

## コード解説
```csharp
Observable
    .Merge(_subject1, _subject2)
    .Subscribe(
        onNext: x => Debug.Log($"next: {x}"),
        onCompleted: _ => Debug.Log("completed.")
    ).AddTo(this);
```
`Observable.Merge`に合成したい複数のObservable（ここでは`_subject1`と`_subject2`）を渡すだけで、それらを1本にまとめたObservableが得られます。

```csharp
_subject1.OnNext("Subject1に流した値"); // next: Subject1に流した値
_subject2.OnNext("Subject2に流した値"); // next: Subject2に流した値
```
どちらのSubjectに値を流しても、Merge後のストリームにはその値がそのまま、呼び出した順番通りに流れてきます。加工や待ち合わせは行われません。

```csharp
// それぞれのSubjectを完了する
// Mergeは合成元のSubjectがすべて完了した時点でcompleted.が呼ばれる
_subject1.OnCompleted();
_subject2.OnCompleted();
```
ここが特に重要なポイントです。`_subject1.OnCompleted()`を呼んだ時点ではまだMerge後のストリームは完了しません。`_subject2.OnCompleted()`が呼ばれ、**両方が完了して初めて**`completed.`が出力されます。

## 実行結果の例
```
next: Subject1に流した値
next: Subject2に流した値
completed.
```

## よくあるつまずきポイント
- 「Mergeは値を組み合わせる」と勘違いしやすいですが、Mergeは組み合わせません。値をそのまま流すだけです。値の組み合わせをしたい場合はZipやCombineLatestを使います。
- 完了のタイミングを「どちらか片方が完了すれば良い」と誤解しがちですが、正しくは「**全ての**合成元が完了する」まで待ちます。
- Concatと混同しやすいですが、Concatは「順番につなげる」ため、後ろのストリームは前のストリームが完了するまで値を流しません。一方Mergeは、どのストリームも最初から同時に値を流せます。

## 理解度チェック
1. `_subject1`にだけ値を3回流し、`_subject2`には一度も値を流さなかった場合、Mergeの`Subscribe`には何回`next`が出力されますか。
2. `_subject1.OnCompleted()`だけを呼んで`_subject2.OnCompleted()`を呼ばなかった場合、`completed.`は出力されますか。理由も答えてください。
3. MergeとConcatの最大の違いを、自分の言葉で1〜2文で説明してください。
