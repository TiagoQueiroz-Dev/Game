using System;
using System.Collections.Generic;
using UnityEngine;

public enum SaoMenuIcon { Person, Party, Message, Map, Settings, Bag, Close, Confirm, Coin }

// Desenha por código (SDF) todos os sprites do menu: círculos, painéis, ícones e a silhueta.
// Os ícones são brancos com sombreamento cinza, então a cor final vem do Image.color.
public static class SaoArt
{
    private static readonly Dictionary<string, Sprite> cache = new Dictionary<string, Sprite>();

    public static Sprite Circle => Get("circle", 128, 128, p => p.Fill(1f, v => Sdf.Circle(v, C, 62f)));

    public static Sprite RingSprite => Get("ring", 128, 128, p => p.Fill(1f, v => Sdf.Ring(v, C, 58f, 6f)));

    // Borda branca grossa com contorno cinza (slot de equipamento do anime). O miolo é vazado.
    public static Sprite SlotRing => Get("slot", 128, 128, p =>
    {
        p.Fill(0.55f, v => Sdf.Circle(v, C, 62f));
        p.Fill(1f, v => Sdf.Circle(v, C, 58f));
        p.Fill(0f, v => Sdf.Circle(v, C, 47f), 0f);
    });

    public static Sprite Triangle => Get("triangle", 64, 64, p =>
        p.Fill(1f, v => Sdf.Polygon(v, new[] { new Vector2(4, 4), new Vector2(60, 32), new Vector2(4, 60) })));

    // Retângulo arredondado para Image.Type.Sliced.
    public static Sprite RoundedRect
    {
        get
        {
            if (cache.TryGetValue("rrect", out Sprite s)) return s;
            Texture2D tex = Paint(64, 64, p => p.Fill(1f, v => Sdf.Box(v, new Vector2(32, 32), new Vector2(31, 31), 14f)));
            s = Sprite.Create(tex, new Rect(0, 0, 64, 64), new Vector2(0.5f, 0.5f), 100f, 0,
                SpriteMeshType.FullRect, new Vector4(20, 20, 20, 20));
            s.name = "SAO rrect";
            s.hideFlags = HideFlags.DontUnloadUnusedAsset;
            cache["rrect"] = s;
            return s;
        }
    }

    public static Sprite Silhouette => Get("silhouette", 128, 256, p =>
    {
        p.Fill(1f, v => Sdf.Circle(v, new Vector2(64, 222), 21f));
        p.Fill(1f, v => Sdf.Box(v, new Vector2(64, 196), new Vector2(8, 10), 2f));
        p.Fill(1f, v => Sdf.Polygon(v, new[] { new Vector2(34, 188), new Vector2(94, 188), new Vector2(86, 112), new Vector2(42, 112) }) - 4f);
        p.Fill(1f, v => Sdf.Capsule(v, new Vector2(35, 182), new Vector2(22, 136), 8f));
        p.Fill(1f, v => Sdf.Capsule(v, new Vector2(22, 136), new Vector2(18, 92), 7f));
        p.Fill(1f, v => Sdf.Capsule(v, new Vector2(93, 182), new Vector2(106, 136), 8f));
        p.Fill(1f, v => Sdf.Capsule(v, new Vector2(106, 136), new Vector2(110, 92), 7f));
        p.Fill(1f, v => Sdf.Box(v, new Vector2(64, 106), new Vector2(23, 12), 5f));
        p.Fill(1f, v => Sdf.Capsule(v, new Vector2(52, 100), new Vector2(49, 54), 10f));
        p.Fill(1f, v => Sdf.Capsule(v, new Vector2(49, 54), new Vector2(47, 14), 8.5f));
        p.Fill(1f, v => Sdf.Capsule(v, new Vector2(76, 100), new Vector2(79, 54), 10f));
        p.Fill(1f, v => Sdf.Capsule(v, new Vector2(79, 54), new Vector2(81, 14), 8.5f));
    });

    public static Sprite MenuIcon(SaoMenuIcon icon)
    {
        return Get("menu_" + icon, 128, 128, p =>
        {
            switch (icon)
            {
                case SaoMenuIcon.Person:
                    DrawPerson(p, new Vector2(64, 64), 1f);
                    break;
                case SaoMenuIcon.Party:
                    DrawPerson(p, new Vector2(80, 70), 0.8f);
                    p.Fill(0f, v => Sdf.Circle(v, new Vector2(52, 66), 34f), 0f); // recorte entre os dois
                    DrawPerson(p, new Vector2(50, 58), 0.85f);
                    break;
                case SaoMenuIcon.Message:
                    p.Fill(1f, v => Mathf.Abs(Sdf.Box(v, C, new Vector2(42, 30), 5f)) - 4f);
                    p.Fill(1f, v => Mathf.Min(
                        Sdf.Capsule(v, new Vector2(26, 88), new Vector2(64, 60), 4f),
                        Sdf.Capsule(v, new Vector2(64, 60), new Vector2(102, 88), 4f)));
                    break;
                case SaoMenuIcon.Map:
                    p.Fill(1f, v => Mathf.Max(
                        Mathf.Min(Sdf.Circle(v, new Vector2(64, 80), 30f),
                            Sdf.Polygon(v, new[] { new Vector2(38, 72), new Vector2(90, 72), new Vector2(64, 18) })),
                        -Sdf.Circle(v, new Vector2(64, 80), 12f)));
                    break;
                case SaoMenuIcon.Settings:
                    p.Fill(1f, v =>
                    {
                        float d = Sdf.Circle(v, C, 32f);
                        for (int i = 0; i < 8; i++)
                        {
                            float a = i * Mathf.PI / 4f;
                            Vector2 dir = new Vector2(Mathf.Cos(a), Mathf.Sin(a));
                            d = Mathf.Min(d, Sdf.Capsule(v, C + dir * 30f, C + dir * 42f, 8f));
                        }
                        return Mathf.Max(d, -Sdf.Circle(v, C, 14f));
                    });
                    break;
                case SaoMenuIcon.Bag:
                    p.Fill(1f, v => Sdf.Box(v, new Vector2(64, 50), new Vector2(38, 32), 12f));
                    p.Fill(1f, v => Mathf.Max(Mathf.Abs(Sdf.Circle(v, new Vector2(64, 82), 20f)) - 5f, 80f - v.y));
                    p.Fill(0.75f, v => Sdf.Box(v, new Vector2(64, 62), new Vector2(10, 7), 3f));
                    break;
                case SaoMenuIcon.Close:
                    p.Fill(1f, v => Mathf.Min(
                        Sdf.Capsule(v, new Vector2(36, 36), new Vector2(92, 92), 9f),
                        Sdf.Capsule(v, new Vector2(36, 92), new Vector2(92, 36), 9f)));
                    break;
                case SaoMenuIcon.Confirm:
                    p.Fill(1f, v => Sdf.Ring(v, C, 34f, 9f));
                    break;
                case SaoMenuIcon.Coin:
                    p.Fill(1f, v => Sdf.Circle(v, C, 50f));
                    p.Fill(0.7f, v => Sdf.Ring(v, C, 38f, 5f));
                    break;
            }
        });
    }

    public static Sprite ItemIcon(ItemData item)
    {
        if (item == null) return null;
        if (item.icon != null) return item.icon;
        return ItemIcon(item.ResolvedShape);
    }

    public static Sprite ItemIcon(ItemIconShape shape)
    {
        return Get("item_" + shape, 128, 128, p =>
        {
            switch (shape)
            {
                case ItemIconShape.Sword: DrawSword(p, 1f); break;
                case ItemIconShape.Dagger: DrawSword(p, 0.7f); break;
                case ItemIconShape.Shield:
                    Vector2[] shield = { new Vector2(22, 106), new Vector2(106, 106), new Vector2(106, 66), new Vector2(64, 16), new Vector2(22, 66) };
                    p.Fill(0.75f, v => Sdf.Polygon(v, shield) - 2f);
                    p.Fill(1f, v => Sdf.Polygon(v, shield) + 9f);
                    p.Fill(0.75f, v => Sdf.Circle(v, new Vector2(64, 72), 13f));
                    break;
                case ItemIconShape.Helmet:
                    p.Fill(1f, v => Mathf.Min(Sdf.Circle(v, new Vector2(64, 70), 42f), Sdf.Box(v, new Vector2(64, 34), new Vector2(42, 14), 4f)));
                    p.Fill(0.3f, v => Sdf.Circle(v, new Vector2(64, 58), 24f));
                    break;
                case ItemIconShape.Coat:
                    p.Fill(1f, v => Sdf.Polygon(v, new[]
                    {
                        new Vector2(40, 108), new Vector2(88, 108), new Vector2(114, 84), new Vector2(102, 58),
                        new Vector2(92, 64), new Vector2(94, 12), new Vector2(34, 12), new Vector2(36, 64),
                        new Vector2(26, 58), new Vector2(14, 84),
                    }));
                    p.Fill(0.45f, v => Sdf.Polygon(v, new[] { new Vector2(50, 108), new Vector2(78, 108), new Vector2(64, 82) }));
                    p.Fill(0.6f, v => Sdf.Capsule(v, new Vector2(64, 82), new Vector2(64, 14), 1.8f));
                    break;
                case ItemIconShape.Gloves:
                    p.Fill(1f, v =>
                    {
                        float d = Sdf.Box(v, new Vector2(62, 54), new Vector2(24, 28), 10f);
                        d = Mathf.Min(d, Sdf.Capsule(v, new Vector2(45, 72), new Vector2(44, 100), 7f));
                        d = Mathf.Min(d, Sdf.Capsule(v, new Vector2(59, 76), new Vector2(59, 110), 7f));
                        d = Mathf.Min(d, Sdf.Capsule(v, new Vector2(73, 74), new Vector2(74, 104), 7f));
                        d = Mathf.Min(d, Sdf.Capsule(v, new Vector2(85, 66), new Vector2(88, 92), 6f));
                        d = Mathf.Min(d, Sdf.Capsule(v, new Vector2(40, 54), new Vector2(26, 74), 7f));
                        return d;
                    });
                    p.Fill(0.7f, v => Sdf.Box(v, new Vector2(62, 22), new Vector2(26, 8), 3f));
                    break;
                case ItemIconShape.Pants:
                    p.Fill(1f, v => Sdf.Polygon(v, new[]
                    {
                        new Vector2(34, 108), new Vector2(94, 108), new Vector2(100, 14), new Vector2(74, 14),
                        new Vector2(64, 74), new Vector2(54, 14), new Vector2(28, 14),
                    }));
                    p.Fill(0.6f, v => Sdf.Box(v, new Vector2(64, 102), new Vector2(31, 5), 2f));
                    break;
                case ItemIconShape.Boots:
                    p.Fill(1f, v => Sdf.Polygon(v, new[]
                    {
                        new Vector2(38, 110), new Vector2(70, 110), new Vector2(70, 50), new Vector2(106, 38),
                        new Vector2(106, 20), new Vector2(38, 20),
                    }) - 2f);
                    p.Fill(0.5f, v => Sdf.Box(v, new Vector2(72, 18), new Vector2(36, 6), 3f));
                    break;
                case ItemIconShape.Ring:
                    p.Fill(1f, v => Sdf.Ring(v, new Vector2(64, 54), 30f, 10f));
                    p.Fill(0.85f, v => Sdf.Polygon(v, new[] { new Vector2(64, 112), new Vector2(84, 92), new Vector2(64, 72), new Vector2(44, 92) }));
                    p.Fill(1f, v => Sdf.Polygon(v, new[] { new Vector2(64, 108), new Vector2(78, 94), new Vector2(64, 92), new Vector2(50, 94) }));
                    break;
                case ItemIconShape.Necklace:
                    p.Fill(1f, v => Mathf.Min(
                        Mathf.Max(Mathf.Abs(Sdf.Circle(v, new Vector2(64, 78), 36f)) - 3f, v.y - 80f),
                        Mathf.Min(Sdf.Capsule(v, new Vector2(28, 80), new Vector2(42, 112), 3f),
                            Sdf.Capsule(v, new Vector2(100, 80), new Vector2(86, 112), 3f))));
                    p.Fill(0.85f, v => Sdf.Polygon(v, new[] { new Vector2(64, 46), new Vector2(80, 28), new Vector2(64, 8), new Vector2(48, 28) }));
                    break;
                case ItemIconShape.Potion:
                    p.Fill(0.85f, v => Mathf.Min(Sdf.Circle(v, new Vector2(64, 46), 32f), Sdf.Box(v, new Vector2(64, 86), new Vector2(11, 16), 3f)));
                    p.Fill(0.6f, v => Sdf.Box(v, new Vector2(64, 106), new Vector2(14, 7), 3f));
                    p.Fill(1f, v => Sdf.Circle(v, new Vector2(52, 56), 8f));
                    break;
                case ItemIconShape.Crystal:
                    p.Fill(1f, v => Sdf.Polygon(v, new[] { new Vector2(64, 114), new Vector2(90, 80), new Vector2(80, 16), new Vector2(48, 16), new Vector2(38, 80) }));
                    p.Fill(0.75f, v => Sdf.Polygon(v, new[] { new Vector2(64, 114), new Vector2(64, 16), new Vector2(48, 16), new Vector2(38, 80) }));
                    break;
                case ItemIconShape.Hide:
                    p.Fill(1f, v => Sdf.Polygon(v, new[]
                    {
                        new Vector2(28, 102), new Vector2(50, 92), new Vector2(64, 106), new Vector2(78, 92),
                        new Vector2(100, 102), new Vector2(92, 70), new Vector2(106, 38), new Vector2(80, 28),
                        new Vector2(64, 14), new Vector2(48, 28), new Vector2(22, 38), new Vector2(36, 70),
                    }));
                    p.Fill(0.8f, v => Sdf.Circle(v, new Vector2(64, 60), 16f));
                    break;
                case ItemIconShape.Ore:
                default:
                    p.Fill(0.75f, v => Sdf.Polygon(v, new[] { new Vector2(26, 40), new Vector2(42, 88), new Vector2(76, 102), new Vector2(104, 70), new Vector2(96, 28), new Vector2(58, 18) }));
                    p.Fill(1f, v => Sdf.Polygon(v, new[] { new Vector2(42, 88), new Vector2(76, 102), new Vector2(104, 70), new Vector2(66, 60) }));
                    break;
            }
        });
    }

    // ---------------------------------------------------------------------------------------

    private static readonly Vector2 C = new Vector2(64, 64);

    private static void DrawPerson(Painter p, Vector2 center, float scale)
    {
        p.Fill(1f, v => Sdf.Circle(v, center + new Vector2(0, 24) * scale, 19f * scale));
        p.Fill(1f, v => Mathf.Max(
            Sdf.Box(v, center + new Vector2(0, -26) * scale, new Vector2(34, 30) * scale, 26f * scale),
            v.y - (center.y - 2f * scale)));
    }

    private static void DrawSword(Painter p, float length)
    {
        Vector2 dir = new Vector2(1, 1).normalized;
        Vector2 side = new Vector2(1, -1).normalized;
        Vector2 guard = new Vector2(40, 40);
        Vector2 tip = guard + dir * (88f * length);

        p.Fill(1f, v => Sdf.Polygon(v, new[]
        {
            guard + side * 7f, tip - dir * 14f + side * 7f, tip, tip - dir * 14f - side * 7f, guard - side * 7f,
        }));
        p.Fill(0.8f, v => Sdf.Capsule(v, guard + dir * 6f, tip - dir * 18f, 1.6f));
        p.Fill(0.6f, v => Sdf.Capsule(v, guard + side * 22f, guard - side * 22f, 5f));
        p.Fill(0.5f, v => Sdf.Capsule(v, guard - dir * 4f, guard - dir * 24f, 4.5f));
        p.Fill(0.65f, v => Sdf.Circle(v, guard - dir * 28f, 7f));
    }

    private static Sprite Get(string key, int w, int h, Action<Painter> draw)
    {
        if (cache.TryGetValue(key, out Sprite s) && s != null) return s;
        Texture2D tex = Paint(w, h, draw);
        s = Sprite.Create(tex, new Rect(0, 0, w, h), new Vector2(0.5f, 0.5f), 100f);
        s.name = "SAO " + key;
        s.hideFlags = HideFlags.DontUnloadUnusedAsset;
        cache[key] = s;
        return s;
    }

    private static Texture2D Paint(int w, int h, Action<Painter> draw)
    {
        Painter painter = new Painter(w, h);
        draw(painter);
        Texture2D tex = new Texture2D(w, h, TextureFormat.RGBA32, true);
        tex.hideFlags = HideFlags.DontUnloadUnusedAsset; // sobrevive à troca de cena
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.filterMode = FilterMode.Trilinear;
        tex.SetPixels(painter.Pixels);
        tex.Apply(true, true);
        return tex;
    }

    private sealed class Painter
    {
        public readonly Color[] Pixels;
        private readonly int w, h;

        public Painter(int w, int h)
        {
            this.w = w;
            this.h = h;
            Pixels = new Color[w * h];
        }

        // Pinta a forma (distância < 0 = dentro) com um tom de cinza. alpha = 0 apaga.
        public void Fill(float gray, Func<Vector2, float> sdf, float alpha = 1f)
        {
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    float d = sdf(new Vector2(x + 0.5f, y + 0.5f));
                    float cover = Mathf.Clamp01(0.5f - d);
                    if (cover <= 0f) continue;

                    int i = y * w + x;
                    Color dst = Pixels[i];
                    if (alpha <= 0f)
                    {
                        dst.a *= 1f - cover;
                        Pixels[i] = dst;
                        continue;
                    }

                    float a = cover * alpha;
                    float outA = a + dst.a * (1f - a);
                    float g = outA > 0f ? (gray * a + dst.r * dst.a * (1f - a)) / outA : gray;
                    Pixels[i] = new Color(g, g, g, outA);
                }
            }
        }
    }

    private static class Sdf
    {
        public static float Circle(Vector2 p, Vector2 c, float r) => (p - c).magnitude - r;

        public static float Ring(Vector2 p, Vector2 c, float r, float thickness) => Mathf.Abs(Circle(p, c, r)) - thickness * 0.5f;

        public static float Capsule(Vector2 p, Vector2 a, Vector2 b, float r)
        {
            Vector2 pa = p - a, ba = b - a;
            float t = Mathf.Clamp01(Vector2.Dot(pa, ba) / Vector2.Dot(ba, ba));
            return (pa - ba * t).magnitude - r;
        }

        public static float Box(Vector2 p, Vector2 c, Vector2 half, float radius)
        {
            Vector2 d = new Vector2(Mathf.Abs(p.x - c.x), Mathf.Abs(p.y - c.y)) - half + new Vector2(radius, radius);
            Vector2 outside = new Vector2(Mathf.Max(d.x, 0f), Mathf.Max(d.y, 0f));
            return outside.magnitude + Mathf.Min(Mathf.Max(d.x, d.y), 0f) - radius;
        }

        public static float Polygon(Vector2 p, Vector2[] v)
        {
            float d = Vector2.Dot(p - v[0], p - v[0]);
            float s = 1f;
            for (int i = 0, j = v.Length - 1; i < v.Length; j = i, i++)
            {
                Vector2 e = v[j] - v[i];
                Vector2 w = p - v[i];
                Vector2 b = w - e * Mathf.Clamp01(Vector2.Dot(w, e) / Vector2.Dot(e, e));
                d = Mathf.Min(d, Vector2.Dot(b, b));
                bool c1 = p.y >= v[i].y, c2 = p.y < v[j].y, c3 = e.x * w.y > e.y * w.x;
                if ((c1 && c2 && c3) || (!c1 && !c2 && !c3)) s = -s;
            }
            return s * Mathf.Sqrt(d);
        }
    }
}
