namespace restful_api_joaodias.Data.Converter.Contract
{
    public interface IParser<O, D>
    {
        public D Parse(O origin);

        public List<D> Parse(List<O> origin);
    }
}
