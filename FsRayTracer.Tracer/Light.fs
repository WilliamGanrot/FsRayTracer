namespace RayTracer.Light

open RayTracer.Point
open RayTracer.Color

[<AutoOpen>]
module Domain =
    type Light = {poistion: Point; intensity: Color}

module Light =

    let create c p =
        {poistion = p; intensity = c}

