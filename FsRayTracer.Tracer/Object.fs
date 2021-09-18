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
open RayTracer.Constnats
open RayTracer.ObjectDomain
open RayTracer.RenderingDomain
open RayTracer.RayDomain


module Object =

    let transform t (object:Object) : Object =
        let t = Transformation.applyToMatrix t object.transform
        { object with transform = t; transformInverse = t |> Matrix.inverse }

    let setMaterial m object =
        {object with material = m}

    let pattern (pattern:Pattern) (object:Object) worldPoint =
        let objectPoint =
            object.transform
            |> Matrix.inverse
            |> Matrix.multiplyPoint worldPoint

        let patternPoint =
            pattern.transform
            |> Matrix.inverse
            |> Matrix.multiplyPoint objectPoint

        Pattern.at patternPoint pattern

    let lighting material light point eyevector normalv inShadow (object:Object) =

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


    let rec findParent child (parents:Object list) =
        match parents with
        | [] -> None 
        | parent::t->
            match parent.shape with
            | Group(parentChildren) when parentChildren |> List.exists (fun x -> x.id = child.id) ->
                parentChildren |> List.find (fun parent -> parent.id = child.id) |> Some
            | Group(_) -> findParent child t
            | _ -> None

    let rec worldToObject object point =
        match object.parent with
        | Some parent ->
            let point' = worldToObject parent point
            object.transformInverse |> Matrix.multiplyPoint point'
        | None -> object.transformInverse |> Matrix.multiplyPoint point

    let rec normalToWorld (object:Object) v : Vector =
        let normal =
            object.transformInverse
            |> Matrix.Transpose
            |> Matrix.multiplyVector v
            |> Vector.withW 0.
            |> Vector.normalize

        let normal' =
            match object.parent with
            | Some parent -> normalToWorld parent normal
            | None -> normal

        normal'

    let normal point (object:Object) =

        let localPoint = worldToObject object point
        let localNormal = object.localNormalAt object.shape localPoint

        normalToWorld object localNormal
