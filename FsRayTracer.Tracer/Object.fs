namespace RayTracer.Object

open System
open RayTracer.Transformation
open RayTracer.Matrix
open RayTracer.Vector
open RayTracer.Point
open RayTracer.Material
open RayTracer.Pattern
open RayTracer.Color
open RayTracer.Helpers
open RayTracer.Light


[<AutoOpen>]
module Domain =
    type Sphere = { id: int; radii: float; }
        
        

    type Shape =
        | Sphere of Sphere
        | Plane

    type Object =
        { transform: Matrix; material: Material; shape: Shape; id: int}

        static member (.=) (o : Object, o2: Object) =
            o.transform .= o2.transform && o.material .= o2.material && o.shape = o2.shape


module Object =


    let sphere =
        let r = System.Random()
        let sphere = Sphere { id = r.Next(); radii = 1. }
        { transform = Matrix.identityMatrix 4; material = Material.standard; shape = sphere; id = r.Next() }

    let glassSphere =
        let r = System.Random()
        let sphere = Sphere { id = r.Next(); radii = 1. }

        let m =
            Material.standard
            |> Material.WithReflectiveIndex 1.5
            |> Material.withTransparency 1.

        { transform = Matrix.identityMatrix 4; material = m; shape = sphere; id = r.Next()}

    let plane =
        let r = new Random()
        let plane = Plane
        { transform = Matrix.identityMatrix 4; material = Material.standard; shape = plane; id = r.Next();}

    let transform t object =
        let t = Transformation.applyToMatrix t object.transform
        {object with transform = t}

    let normal point object =

        let objectPoint =
            (Matrix.inverse object.transform)
            |> Matrix.multiplyPoint point

        let objectNormal =
            match object.shape with
            | Sphere _ -> (objectPoint - (Point.create 0. 0. 0.))
            | Plane ->    Vector.create 0. 1. 0.

        let worldNormal =
            object.transform
            |> Matrix.inverse
            |> Matrix.Transpose
            |> Matrix.multiplyVector objectNormal

        Vector.normalize {worldNormal with W = 0.}

    let setMaterial m object =
        {object with material = m}

    let pattern (pattern:Pattern) object worldPoint =
        let objectPoint =
            object.transform
            |> Matrix.inverse
            |> Matrix.multiplyPoint worldPoint

        let patternPoint =
            pattern.transform
            |> Matrix.inverse
            |> Matrix.multiplyPoint objectPoint

        Pattern.at patternPoint pattern

    let lighting material light point eyevector normalv inShadow object =

        let color =
            match material.pattern with
            | Some p        -> pattern p object point 
            | None          -> material.color
        
        let effectiveColor = color * light.intensity
        let lightv = (light.poistion - point) |> Vector.normalize
        let ambient = effectiveColor |> Color.mulitplyByScalar material.ambient
        let lightDotNormal = Vector.dot lightv normalv

        let diffuse =
            effectiveColor
            |> Color.mulitplyByScalar (material.diffuse * lightDotNormal)

        let reflectDotEye =
            Vector.reflect normalv (lightv * -1.)
            |> Vector.dot eyevector

        match inShadow with
        | true -> ambient
        | false ->
            match lightDotNormal with
            | v when v < 0. -> ambient
            | _ when reflectDotEye <= 0. -> ambient + diffuse
            | _ ->
                let factor = Math.Pow(reflectDotEye, material.shininess)
                ambient + diffuse + (light.intensity |> Color.mulitplyByScalar (material.specular * factor))


