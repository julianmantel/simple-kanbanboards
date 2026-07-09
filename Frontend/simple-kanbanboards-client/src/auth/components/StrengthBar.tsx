interface StrengthBarProps {
  value: string;
}

function checkStrength(val: string): { score: number; color: string; width: string } {
  let score = 0;
  if (val.length >= 8) score++;
  if (/[A-Z]/.test(val)) score++;
  if (/[0-9]/.test(val)) score++;
  if (/[^A-Za-z0-9]/.test(val)) score++;
  const widths = ['0%', '25%', '50%', '75%', '100%'];
  const colors = ['', '#E24B4A', '#BA7517', '#1D9E75', '#5DCAA5'];
  return { score, color: colors[score], width: widths[score] };
}

export default function StrengthBar({ value }: StrengthBarProps) {
  const { width, color } = checkStrength(value);

  if (!value) return null;

  return (
    <div className="h-1 rounded overflow-hidden bg-[#2a2a32] mt-1.5">
      <div
        className="h-full rounded transition-all duration-300"
        style={{ width, background: color }}
      />
    </div>
  );
}
