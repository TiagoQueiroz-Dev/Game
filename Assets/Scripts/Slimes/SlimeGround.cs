using UnityEngine;

// Utilitário para achar a altura do chão em um ponto (usa o Terrain se existir, senão Raycast).
public static class SlimeGround
{
    public static bool TryGetHeight(Vector3 position, out float height)
    {
        Terrain terrain = Terrain.activeTerrain;
        if (terrain != null)
        {
            height = terrain.SampleHeight(position) + terrain.GetPosition().y;
            return true;
        }

        Vector3 origin = position + Vector3.up * 50f;
        RaycastHit[] hits = Physics.RaycastAll(origin, Vector3.down, 200f, ~0, QueryTriggerInteraction.Ignore);
        float best = float.MaxValue;
        height = position.y;
        bool found = false;

        foreach (RaycastHit hit in hits)
        {
            // Ignora outros slimes para não "subir" em cima deles.
            if (hit.collider.GetComponentInParent<Slime>() != null) continue;
            if (hit.distance < best)
            {
                best = hit.distance;
                height = hit.point.y;
                found = true;
            }
        }

        return found;
    }
}
