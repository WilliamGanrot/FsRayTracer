namespace RayTracer.Shape

open System

[<AutoOpen>]
module Domain =
    type Sphere = {id:int; radii:float}

module Shape =

    let sphere =
        let r = new Random()
        {id = r.Next(); radii = 1.}

