namespace RayTracer.Matrix

open RayTracer.Vector
open RayTracer.Point
open RayTracer.Helpers

[<AutoOpen>]
module Domain =
    type Matrix =
        {dimensions: int; entries: float [,]}

        static member (*) (a: Matrix, b: Matrix) =
            let entries = 
                [for rowindex in 0..a.dimensions - 1 do
                    [for colindex in 0..b.dimensions - 1 do
                        let row = a.entries.[rowindex,*]
                        let col = b.entries.[*,colindex]
                        (Array.map2(fun x y -> x * y) row col) |> Array.sum ]]

            {a with entries = array2D entries}

        static member (.=) (a: Matrix, b: Matrix) =

            let aList = a.entries |> Seq.cast<float> |> Seq.toList
            let bList = b.entries |> Seq.cast<float> |> Seq.toList

            Seq.forall2(fun x y -> FloatHelper.equal x y) aList bList


module Matrix =
    let make (m: float list list) : Matrix =
        {dimensions = m.Length; entries = array2D m}

    let identityMatrix (size:int) =
        [0..size-1]
        |> List.map (fun x -> [0..size-1] |> List.map (fun x2 -> if x2 = x then 1. else 0.))
        |> make

    let multiplyByList (l: float []) (m:Matrix) =
        [for i in 0..l.Length - 1 do
            let row = m.entries.[i,*]
            (Array.map2(fun x y -> x * y) l row) |> Array.sum ]
   
    let multiplyVector (m:Matrix) (v:Vector) =
        let l = m |> multiplyByList [|v.X; v.Y; v.Z; v.W|]
        Vector.create l.[0] l.[1] l.[2]

    let multiplyPoint (m:Matrix) (p:Point) =
        let l = m |> multiplyByList [|p.X; p.Y; p.Z; p.W|]
        Point.create l.[0] l.[1] l.[2]

    let Transpose (a:Matrix) =
        let e = 
            [for rowindex in [0..a.dimensions - 1] do a.entries.[*,rowindex]]
            |> array2D
        {a with entries = e}
    
    let submatrix (rowIndex:int) (colIndex:int) (a:Matrix) : Matrix =

        let removeIndexFromList popIndex l =
            l
            |> List.mapi (fun i cell -> cell, i)
            |> List.choose (fun (x,i) -> if i <> popIndex then Some x else None)

        let rowlist = [for i in 0..a.dimensions - 1 do a.entries.[i,*]]

        rowlist
        |> List.map (fun row -> row |> Array.toList |> removeIndexFromList colIndex)
        |> removeIndexFromList rowIndex
        |> make
        
    let rec determinant (a:Matrix) =

        match a.dimensions with
        | d when d = 2 ->
            a.entries.[0,0] * a.entries.[1,1] - a.entries.[0,1] * a.entries.[1,0]
        | d when d > 2 ->
            let targetRowIndex = 0

            a.entries.[targetRowIndex,*]
            |> Array.toList
            |> List.mapi(fun i x -> x * cofactor targetRowIndex i a)
            |> List.sum
        | _ -> failwith "to small matrix"

    and minor (rowIndex:int) (colIndex:int) (a:Matrix) =
        a
        |> submatrix rowIndex colIndex
        |> determinant
        
    and cofactor (rowIndex:int) (colIndex:int) (a:Matrix) =

        let minor = a |> minor rowIndex colIndex

        match rowIndex + colIndex with
        | IsEven -> minor
        | _ -> minor * -1.


    let invertable m =
        not (FloatHelper.equal 0.  (m |> determinant))

    let inverse (m:Matrix) =
        match invertable m with
        | false -> failwith "not invertable"
        | true ->
            let inversed =
                [for row in 0..m.dimensions - 1 do
                    [for col in 0..m.dimensions - 1 do
                        let c = cofactor row col m
                        let x = c / determinant m
                        x ]]
            make inversed |> Transpose
