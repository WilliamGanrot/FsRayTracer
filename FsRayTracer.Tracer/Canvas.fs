namespace Raytracer.Canvas
open RayTracer.Color

[<AutoOpen>]
module Domain =

    type ColorCordinat = {X: int; Y:int; Color:Color;}

    type Canvas = {Width: int; Height: int; Pixels: Color [,] }


module Canvas =

    let makeCanvas w h = {Width = w; Height = h; Pixels = Array2D.create h w Color.black }
    let getPixel x y canvas = Array2D.get canvas.Pixels y x
    let setPixel x y color canvas = Array2D.set canvas.Pixels y x color
    let toPPM canvas =
 
        let header = ["P3"; $"{canvas.Width} {canvas.Height}"; "255"]

        let toPixelRows (pixelMap: string [,])  =
            [ for row in 0..canvas.Height - 1 do pixelMap.[row,*] |> Array.toList ]
            |> Seq.map(fun x -> String.concat " " x)
            |> Seq.toList
            
        let divideLongRow (row: string) : string list =
            let rec loop (listacc : string list) (acc:string) (row: string list) =
                match row with
                | h::t when acc.Length = 0 ->                   loop listacc h t
                | h::t when acc.Length + h.Length >= 70 ->      loop (listacc @ [acc]) h t
                | h::t when acc.Length + h.Length + 1 <= 70 ->  loop listacc $"{acc} {h}" t
                | [] when acc.Length > 0 ->                     listacc @ [acc]
                | _ ->                                         listacc

            row.Split " " |> Array.toList |> loop [] ""

        let rec divideLongRows (rows: string list) : string list =
            match rows with
            | row::t -> divideLongRow row @ divideLongRows t
            | [] -> []

        let pixelData =
            canvas.Pixels
            |> Array2D.map Color.toRgb
            |> toPixelRows
            |> divideLongRows

        let lines = header @ pixelData
        (String.concat "\n" lines) + "\n"

    let cordinats (m:Canvas) : ColorCordinat list =
        [ for y in 0..m.Height - 1 do
             for x in 0..m.Width - 1 do
                 { X = x; Y = y; Color = m.Pixels.[y,x]} ]