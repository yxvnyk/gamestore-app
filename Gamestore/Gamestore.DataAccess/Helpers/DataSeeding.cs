using Gamestore.DataAccess.Entities;

namespace Gamestore.DataAccess.Helpers;

internal static class DataSeeding
{
    // Platform GUIDs
    public static readonly Guid MobilePlatformId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
    public static readonly Guid BrowserPlatformId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
    public static readonly Guid DesktopPlatformId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");
    public static readonly Guid ConsolePlatformId = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd");

    // Genre GUIDs
    public static readonly Guid StrategyGenreId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    public static readonly Guid RTSGenreId = Guid.Parse("11111111-1111-1111-1111-111111111112");
    public static readonly Guid TBSGenreId = Guid.Parse("11111111-1111-1111-1111-111111111113");
    public static readonly Guid RPGGenreId = Guid.Parse("11111111-1111-1111-1111-111111111114");
    public static readonly Guid SportsGenreId = Guid.Parse("11111111-1111-1111-1111-111111111115");
    public static readonly Guid RacesGenreId = Guid.Parse("22222222-2222-2222-2222-222222222222");
    public static readonly Guid RallyGenreId = Guid.Parse("22222222-2222-2222-2222-222222222223");
    public static readonly Guid FormulaGenreId = Guid.Parse("22222222-2222-2222-2222-222222222224");
    public static readonly Guid OffroadGenreId = Guid.Parse("22222222-2222-2222-2222-222222222225");
    public static readonly Guid ArcadeGenreId = Guid.Parse("33333333-3333-3333-3333-333333333333");
    public static readonly Guid ActionGenreId = Guid.Parse("44444444-4444-4444-4444-444444444444");
    public static readonly Guid FPSGenreId = Guid.Parse("44444444-4444-4444-4444-444444444445");
    public static readonly Guid TPSGenreId = Guid.Parse("44444444-4444-4444-4444-444444444446");
    public static readonly Guid AdventureGenreId = Guid.Parse("55555555-5555-5555-5555-555555555555");
    public static readonly Guid PuzzleGenreId = Guid.Parse("66666666-6666-6666-6666-666666666666");

    public static PlatformEntity[] GetPlatforms() =>
    [
        new PlatformEntity { Id = MobilePlatformId,  Type = "Mobile" },
        new PlatformEntity { Id = BrowserPlatformId, Type = "Browser" },
        new PlatformEntity { Id = DesktopPlatformId, Type = "Desktop" },
        new PlatformEntity { Id = ConsolePlatformId, Type = "Console" },
    ];

    public static GenreEntity[] GetGenres() =>
    [
        new GenreEntity { Id = StrategyGenreId, Name = "Strategy" },
        new GenreEntity { Id = RTSGenreId, Name = "RTS", ParentGenreId = StrategyGenreId },
        new GenreEntity { Id = TBSGenreId, Name = "TBS", ParentGenreId = StrategyGenreId },
        new GenreEntity { Id = RPGGenreId, Name = "RPG" },
        new GenreEntity { Id = SportsGenreId, Name = "Sports" },
        new GenreEntity { Id = RacesGenreId, Name = "Races" },
        new GenreEntity { Id = RallyGenreId, Name = "Rally", ParentGenreId = RacesGenreId },
        new GenreEntity { Id = FormulaGenreId, Name = "Formula", ParentGenreId = RacesGenreId },
        new GenreEntity { Id = OffroadGenreId, Name = "Off-road", ParentGenreId = RacesGenreId },
        new GenreEntity { Id = ArcadeGenreId, Name = "Arcade" },
        new GenreEntity { Id = ActionGenreId, Name = "Action" },
        new GenreEntity { Id = FPSGenreId, Name = "FPS", ParentGenreId = ActionGenreId },
        new GenreEntity { Id = TPSGenreId, Name = "TPS", ParentGenreId = ActionGenreId },
        new GenreEntity { Id = AdventureGenreId, Name = "Adventure" },
        new GenreEntity { Id = PuzzleGenreId, Name = "Puzzle & Skill" }
    ];
}