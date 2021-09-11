module CanvasTests

open System
open Raytracer.Canvas
open RayTracer.Helpers
open RayTracer.ObjectDomain


open Xunit
open RayTracer.Color

[<Fact>]
let ``Creating canvas`` () =

    let c = Canvas.makeCanvas 10 20
    Assert.Equal(c.Width, 10)
    Assert.Equal(c.Height, 20)

    c.Pixels |> Seq.cast<Color> |> Seq.forall(fun c -> c = Color.black) |> Assert.True

[<Fact>]
let ``Writing pixels to canvas`` () =

    let c = Canvas.makeCanvas 10 20
    let red = Color.red

    Canvas.setPixel 2 3 red c
    Assert.Equal(Canvas.getPixel 2 3 c, red)


[<Fact>]
let ``Constructing the PPM header`` () =

    let c = Canvas.makeCanvas 5 3
    let ppmLines = Canvas.toPPM c |> lines
    Assert.Equal("P3", ppmLines.[0])
    Assert.Equal("5 3", ppmLines.[1])
    Assert.Equal("255", ppmLines.[2])
   

[<Fact>]
let ``Constructing the PPM pixel data`` () =

    let c = Canvas.makeCanvas 5 3
    let c1 = { Red = 1.5; Green = 0.0; Blue = 0.0 }
    let c2 = { Red = 0.0; Green = 0.5; Blue = 0.0 }
    let c3 = { Red = -0.5; Green = 0.0; Blue = 1.0 }

    Canvas.setPixel 0 0 c1 c
    Canvas.setPixel 2 1 c2 c
    Canvas.setPixel 4 2 c3 c

    let ppmLines = Canvas.toPPM c |> lines
    Assert.Equal("255 0 0 0 0 0 0 0 0 0 0 0 0 0 0", ppmLines.[3])
    Assert.Equal("0 0 0 0 0 0 0 128 0 0 0 0 0 0 0", ppmLines.[4])
    Assert.Equal("0 0 0 0 0 0 0 0 0 0 0 0 0 0 255", ppmLines.[5])
   

[<Fact>]
let ``Splitting long lines in PPM files``() =
    let color = { Red = 1.0; Green = 0.8; Blue = 0.6 }
    let canvas = Canvas.makeCanvas 10 2

    Array2D.iteri (fun y x _c -> Canvas.setPixel x y color canvas) canvas.Pixels

    let ppm = canvas |> Canvas.toPPM |> lines

    Assert.Equal("255 204 153 255 204 153 255 204 153 255 204 153 255 204 153 255 204", ppm.[3])
    Assert.Equal("153 255 204 153 255 204 153 255 204 153 255 204 153", ppm.[4])
    Assert.Equal("255 204 153 255 204 153 255 204 153 255 204 153 255 204 153 255 204", ppm.[5])
    Assert.Equal("153 255 204 153 255 204 153 255 204 153 255 204 153", ppm.[6])

[<Fact>]
let ``ppm files are terminated by new charecter``() =
    let ppm =
        Canvas.makeCanvas 5 3
        |> Canvas.toPPM 

    ppm.EndsWith "\n" |> Assert.True