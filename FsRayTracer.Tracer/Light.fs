namespace RayTracer.Light

open RayTracer.RenderingDomain


module Light =

    let create c p =
        { poistion = p; intensity = c }

