<Query Kind="FSharpProgram" />

///Taken from this article: https://www.cogitoergofun.io/merge-sort-in-fsharp/

let mergeWithLoop c d =
    let cLength = Array.length c
    let dLength = Array.length d
    let bLength = cLength + dLength
    let mutable i = 0
    let mutable j = 0

    let b = Array.zeroCreate bLength
    for k in 0 .. bLength - 1 do
        if i < cLength && (j >= dLength || c[i] <= d[j]) then
            b[k] <- c[i]
            i <- i + 1
        else
            b[k] <- d[j]
            j <- j + 1
    b

let merge c d =
    let cLength = Array.length c
    let dLength = Array.length d
    let bLength = cLength + dLength
    let b = Array.zeroCreate bLength

    let rec mergeItem i j k =
        if k >= bLength then
            b
        elif i < cLength && (j >= dLength || c[i] <= d[j]) then
            b[k] <- c[i]
            mergeItem (i + 1) j (k + 1)
        else
            b[k] <- d[j]
            mergeItem i (j + 1) (k + 1)

    mergeItem 0 0 0

let rec mergeSortWithLoop a =
    if Array.length a <= 1 then
        a
    else
        let halfLength = Array.length a / 2
        mergeWithLoop 
            (mergeSortWithLoop a[.. halfLength - 1])
            (mergeSortWithLoop a[halfLength ..])

let rec mergeSort a =
    if Array.length a <= 1 then
        a
    else
        let halfLength = Array.length a / 2
        let c = mergeSort a[.. halfLength - 1]
        let d = mergeSort a[halfLength ..]
        merge c d

let quickBenchmark a sorter =
    let stopWatch = System.Diagnostics.Stopwatch()
    stopWatch.Start()
    let b = sorter a
    stopWatch.Stop()

    $"Time to sort {Array.length b} items: {stopWatch.ElapsedMilliseconds} ms"

[<EntryPoint>]
let main _ =
    let rand = System.Random()
    let a = Array.init 50_000 (fun _ -> rand.Next())

    printfn "Merge-Sort with loop - %s" (quickBenchmark a mergeSortWithLoop)
    printfn "Merge-Sort with recursion - %s" (quickBenchmark a mergeSort)

    0