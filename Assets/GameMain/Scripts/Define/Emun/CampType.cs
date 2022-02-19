namespace ST
{
    /// <summary>
    /// 阵营类型。
    /// </summary>
    public enum CampType : byte
    {
        Unknown = 0,

        /// <summary>
        /// 玩家阵营。
        /// </summary>
        Player,

        /// <summary>
        /// 敌人阵营。
        /// </summary>
        Enemy,

        /// <summary>
        /// 中立阵营。
        /// </summary>
        Neutral
    }
}
