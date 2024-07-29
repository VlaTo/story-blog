namespace StoryBlog.Web.Identity.Client;

public class Parameters : List<KeyValuePair<string, string>>
{
    public IEnumerable<string> this[string index]
    {
        get
        {
            return this
                .Where(kvp => kvp.Key.Equals(index))
                .Select(kvp => kvp.Value);
        }
    }

    public Parameters()
    {
    }

    public Parameters(IEnumerable<KeyValuePair<string, string>> collection)
        : base(collection)
    {
    }

    public void Add(string key, string value, ParameterReplaceBehavior parameterReplace = ParameterReplaceBehavior.None)
    {
        if (String.IsNullOrEmpty(key))
        {
            throw new ArgumentNullException(nameof(key));
        }

        if (ParameterReplaceBehavior.None == parameterReplace)
        {
            Add(new KeyValuePair<string, string>(key, value));
            return;
        }

        var existingItems = this
            .Where(i => i.Key == key)
            .ToList();

        if (1 < existingItems.Count && ParameterReplaceBehavior.Single == parameterReplace)
        {
            throw new InvalidOperationException("More than one item found to replace.");
        }

        existingItems.ForEach(item => Remove(item));

        Add(new KeyValuePair<string, string>(key, value));
    }

    public void AddOptional(string key, string? value, bool allowDuplicates = false)
    {
        if (String.IsNullOrEmpty(key))
        {
            throw new ArgumentNullException(nameof(key));
        }

        if (String.IsNullOrEmpty(value))
        {
            return;
        }

        if (false == allowDuplicates && ContainsKey(key))
        {
            throw new InvalidOperationException($"Duplicate parameter: {key}");
        }

        Add(key, value);
    }

    public void AddRequired(string key, string? value, bool allowDuplicates = false, bool allowEmptyValue = false)
    {
        if (String.IsNullOrEmpty(key))
        {
            throw new ArgumentNullException(nameof(key));
        }

        var valuePresent = false == String.IsNullOrEmpty(value);
        var parameterPresent = ContainsKey(key);

        if (!valuePresent && !parameterPresent && !allowEmptyValue)
        {
            // Don't throw if we have a value already in the parameters
            // to make it more convenient for callers.
            throw new ArgumentException("Parameter is required", key);
        }

        if (valuePresent && parameterPresent && !allowDuplicates)
        {
            if (this[key].Contains(value))
            {
                // The parameters are already in the desired state (the required
                // parameter key already has the specified value), so we don't
                // throw an error
                return;
            }

            throw new InvalidOperationException($"Duplicate parameter: {key}");
        }

        if (valuePresent || allowEmptyValue)
        {
            Add(key, value!);
        }
    }

    public IEnumerable<string> GetValues(string name) => this[name];

    public bool ContainsKey(string key) => this.Any(k => String.Equals(k.Key, key));

    public Parameters Merge(Parameters? additionalValues = null)
    {
        if (additionalValues != null)
        {
            var merged = this
                .Concat(additionalValues.Where(add => !ContainsKey(add.Key)))
                .Select(s => new KeyValuePair<string, string>(s.Key, s.Value));

            return new Parameters(merged.ToList());
        }

        return this;
    }
}