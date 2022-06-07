namespace MTGView.Blazor.Server.UrlHashing;

/// <summary>
/// Defines signatures for methods to encode and decode urls via a simple hashing algorithm
/// </summary>
public interface IUrlHasher
{
    #region Decoding Methods
    /// <summary>
    /// 
    /// </summary>
    /// <param name="hash">The hashed string that we want to decode</param>
    /// <returns></returns>
    Int32 DecodeSingleValue(String hash);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="hash">The hashed string that we want to decode</param>
    /// <returns></returns>
    Int64 DecodeSingleLong(String hash);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="hash">The hashed string that we want to decode</param>
    /// <returns></returns>
    (Boolean isSuccessful, Int32 value) TryDecodeSingleValue(String hash);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="hash">The hashed string that we want to decode</param>
    /// <returns></returns>
    (Boolean isSuccessful, Int64 value) TryDecodeSingleLong(string hash);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="hash">The hashed string that we want to decode</param>
    /// <returns></returns>
    Int32[] DecodeValuesAsIntegers(String hash);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="hash">The hashed string that we want to decode</param>
    /// <returns></returns>
    Int64[] DecodeValuesAsLongs(String hash);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="hash">The hashed string that we want to decode</param>
    /// <returns></returns>
    (Boolean isSuccessful, Int64 value) TryDecodeValuesAsLongs(String hash);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="hash">The hashed string that we want to decode</param>
    /// <returns></returns>
    String DecodeHexValue(String hash);
    #endregion
    #region Integer Encoding Methods
    /// <summary>
    /// Encodes the provided <paramref name="number"/> with a hash and a salt
    /// </summary>
    /// <param name="number">The integer we want to encode</param>
    /// <returns>The hashed result</returns>
    String EncodeInteger(Int32 number);

    /// <summary>
    /// Encodes the provided <paramref name="numbers"/> with a hash and a salt
    /// </summary>
    /// <param name="numbers">The integers we want to encode</param>
    /// <returns>The hashed result</returns>
    String EncodeIntegers(IEnumerable<Int32> numbers);

    /// <summary>
    /// Encodes the provided <paramref name="numbers"/> with a hash and a salt
    /// </summary>
    /// <param name="numbers">A nondescript array of integers </param>
    /// <returns>The hashed result</returns>
    String EncodeIntegers(params Int32[] numbers);
    #endregion
    #region Long Encoding Methods
    String EncodeLong(Int64 number);

    String EncodeLongs(IEnumerable<Int64> numbers);

    String EncodeLongs(params Int64[] numbers);
    #endregion
    #region String Encoding Methods

    String EncodeHex(String hex);

    #endregion
}