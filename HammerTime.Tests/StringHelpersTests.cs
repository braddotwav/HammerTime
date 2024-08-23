using HammerTime.Helpers;

namespace HammerTime.Tests;

public class StringHelpersTests
{
    [Fact]
    public void GetVMFFileName_GivenHammerTitle_ReturnsFileName()
    {
        string input = "Hammer - [C:\\Users\\PC\\Desktop\\test.vmf - Right]";
        string expected = "test.vmf";

        string result = StringHelpers.GetVMFFileName(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void TryGetVMFFileName_GivenHammerTitle_ReturnsTrueWithFileName()
    {
        string input = "Hammer - [C:\\Users\\PC\\Desktop\\test.vmf - Right]";
        string expected = "test.vmf";

        bool result = StringHelpers.TryGetVMFFileName(input, out string outResult);

        Assert.True(result);
        Assert.Equal(expected, outResult);
    }

    [Fact]
    public void IsProcessNameExecutableInDirectory_HammerAndHammerPlusPlus_ReturnsTrue()
    {
        string[] processes = ["hammer", "hammerplusplus"];
        string hammer = @"E:\Program Files (x86)\Steam\steamapps\common\GarrysMod\bin\hammer.exe";
        string hammerplusplus = @"E:\Program Files (x86)\Steam\steamapps\common\GarrysMod\bin\win64\hammerplusplus.exe";

        bool hammerResult = StringHelpers.IsProcessNameExecutableInDirectory(hammer, processes);
        bool hammerplusplusResult = StringHelpers.IsProcessNameExecutableInDirectory(hammerplusplus, processes);

        Assert.True(hammerResult);
        Assert.True(hammerplusplusResult);
    }
}
