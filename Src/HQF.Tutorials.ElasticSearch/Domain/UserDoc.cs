namespace HQF.Tutorials.ElasticSearch.Domain
{

    public    class UserDoc
    {
        
        public int Id { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
    
        public string NickName { get; set; }
        /// <summary>
        /// 是否艺术家
        /// </summary>
        public bool IsArtist { get; set; }
        /// <summary>
        /// 创建的商品数
        /// </summary>
        public int ProductCount { get; set; }
    }
}
